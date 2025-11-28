using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PortfolioAnalyzer.DataSync.Data;
using PortfolioAnalyzer.DataSync.Services;

namespace PortfolioAnalyzer.DataSync.Functions;

public class DataSyncFunction
{
    private readonly ILogger<DataSyncFunction> _logger;
    private readonly PortfolioDbContext _dbContext;
    private readonly FmpApiService _fmpApiService;

    public DataSyncFunction(
        ILogger<DataSyncFunction> logger,
        PortfolioDbContext dbContext,
        FmpApiService fmpApiService)
    {
        _logger = logger;
        _dbContext = dbContext;
        _fmpApiService = fmpApiService;
    }

    /// <summary>
    /// Timer trigger function that syncs financial data from FMP API to PostgreSQL
    /// Runs daily at 6:00 AM UTC (configurable via tickers.json)
    /// Cron format: "0 0 6 * * *" = At 06:00:00 AM every day
    /// </summary>
    [Function("DataSyncFunction")]
    public async Task Run([TimerTrigger("0 0 6 * * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("Data sync function started at: {Time}", DateTime.UtcNow);

        try
        {
            // Load tickers from configuration file
            var tickers = await LoadTickersAsync();
            if (tickers == null || tickers.Length == 0)
            {
                _logger.LogWarning("No tickers found in configuration");
                return;
            }

            _logger.LogInformation("Syncing data for {Count} tickers", tickers.Length);

            var successCount = 0;
            var failureCount = 0;

            foreach (var ticker in tickers)
            {
                try
                {
                    _logger.LogInformation("Processing ticker: {Ticker}", ticker);

                    // Fetch data from FMP API
                    var security = await _fmpApiService.GetSecurityDataAsync(ticker);
                    if (security == null)
                    {
                        _logger.LogWarning("Failed to fetch data for {Ticker}", ticker);
                        failureCount++;
                        continue;
                    }

                    // Upsert security data to database
                    await UpsertSecurityAsync(security);
                    successCount++;

                    _logger.LogInformation("Successfully synced data for {Ticker}", ticker);

                    // Rate limiting: FMP free tier allows 5 requests/minute
                    // Add delay to stay within limits
                    await Task.Delay(TimeSpan.FromSeconds(13)); // ~4.6 requests/minute
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing ticker {Ticker}", ticker);
                    failureCount++;
                }
            }

            _logger.LogInformation(
                "Data sync completed. Success: {SuccessCount}, Failures: {FailureCount}",
                successCount,
                failureCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Critical error in data sync function");
            throw;
        }
    }

    private async Task<string[]> LoadTickersAsync()
    {
        try
        {
            var tickersFilePath = Path.Combine(AppContext.BaseDirectory, "tickers.json");
            if (!File.Exists(tickersFilePath))
            {
                _logger.LogError("tickers.json file not found at {Path}", tickersFilePath);
                return Array.Empty<string>();
            }

            var json = await File.ReadAllTextAsync(tickersFilePath);
            var config = JsonSerializer.Deserialize<TickersConfiguration>(json);

            return config?.Tickers ?? Array.Empty<string>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading tickers configuration");
            return Array.Empty<string>();
        }
    }

    private async Task UpsertSecurityAsync(Shared.Models.Security security)
    {
        var existingSecurity = await _dbContext.Securities
            .Include(s => s.Fundamentals)
            .FirstOrDefaultAsync(s => s.Symbol == security.Symbol);

        if (existingSecurity == null)
        {
            // Insert new security
            _logger.LogInformation("Inserting new security: {Symbol}", security.Symbol);
            _dbContext.Securities.Add(security);
        }
        else
        {
            // Update existing security
            _logger.LogInformation("Updating existing security: {Symbol}", security.Symbol);

            existingSecurity.Name = security.Name;
            existingSecurity.Isin = security.Isin;
            existingSecurity.Sector = security.Sector;
            existingSecurity.Industry = security.Industry;
            existingSecurity.Exchange = security.Exchange;
            existingSecurity.Currency = security.Currency;

            if (security.Fundamentals != null)
            {
                if (existingSecurity.Fundamentals == null)
                {
                    existingSecurity.Fundamentals = security.Fundamentals;
                }
                else
                {
                    // Update fundamental data
                    var fundamentals = existingSecurity.Fundamentals;
                    var newFundamentals = security.Fundamentals;

                    fundamentals.MarketCap = newFundamentals.MarketCap;
                    fundamentals.Revenue = newFundamentals.Revenue;
                    fundamentals.FreeCashFlow = newFundamentals.FreeCashFlow;
                    fundamentals.RevenueGrowth = newFundamentals.RevenueGrowth;
                    fundamentals.EarningsGrowth = newFundamentals.EarningsGrowth;
                    fundamentals.DividendYield = newFundamentals.DividendYield;
                    fundamentals.DividendPerShare = newFundamentals.DividendPerShare;
                    fundamentals.PE = newFundamentals.PE;
                    fundamentals.PB = newFundamentals.PB;
                    fundamentals.ROE = newFundamentals.ROE;
                    fundamentals.DebtToEquity = newFundamentals.DebtToEquity;
                    fundamentals.LastUpdated = DateTime.UtcNow;
                }
            }
        }

        await _dbContext.SaveChangesAsync();
    }

    private class TickersConfiguration
    {
        public string[] Tickers { get; set; } = Array.Empty<string>();
        public string? SyncSchedule { get; set; }
        public string? Description { get; set; }
    }
}
