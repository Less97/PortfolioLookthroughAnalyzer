using Microsoft.EntityFrameworkCore;
using PortfolioAnalyzer.Api.Data;
using PortfolioAnalyzer.Shared.Interfaces;
using PortfolioAnalyzer.Shared.Models;

namespace PortfolioAnalyzer.Api.Services;

public class PostgresPortfolioService : IPortfolioService
{
    private readonly PortfolioDbContext _context;
    private readonly ILogger<PostgresPortfolioService> _logger;

    public PostgresPortfolioService(
        PortfolioDbContext context,
        ILogger<PostgresPortfolioService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Portfolio> GetPortfolioAsync()
    {
        try
        {
            // Get the first portfolio (or create default if none exists)
            var portfolio = await _context.Portfolios
                .Include(p => p.Positions)
                    .ThenInclude(pos => pos.Security)
                        .ThenInclude(s => s!.Fundamentals)
                .FirstOrDefaultAsync();

            if (portfolio == null)
            {
                _logger.LogInformation("No portfolio found, creating default portfolio");
                portfolio = await CreateDefaultPortfolioAsync();
            }

            return portfolio;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving portfolio");
            throw;
        }
    }

    public async Task<Portfolio> UpdatePortfolioAsync(Portfolio portfolio)
    {
        try
        {
            var existingPortfolio = await _context.Portfolios
                .Include(p => p.Positions)
                    .ThenInclude(pos => pos.Security)
                        .ThenInclude(s => s!.Fundamentals)
                .FirstOrDefaultAsync(p => p.Id == portfolio.Id);

            if (existingPortfolio == null)
            {
                _logger.LogInformation("Portfolio {PortfolioId} not found, creating new", portfolio.Id);

                // Ensure all securities exist
                foreach (var position in portfolio.Positions)
                {
                    if (position.Security != null)
                    {
                        await EnsureSecurityExistsAsync(position.Security);
                    }
                }

                portfolio.LastUpdated = DateTime.UtcNow;
                _context.Portfolios.Add(portfolio);
            }
            else
            {
                // Update existing portfolio
                existingPortfolio.Name = portfolio.Name;
                existingPortfolio.Cash = portfolio.Cash;
                existingPortfolio.LastUpdated = DateTime.UtcNow;

                // Remove old positions
                _context.Positions.RemoveRange(existingPortfolio.Positions);

                // Add updated positions
                foreach (var position in portfolio.Positions)
                {
                    if (position.Security != null)
                    {
                        await EnsureSecurityExistsAsync(position.Security);
                    }

                    position.LastUpdated = DateTime.UtcNow;
                    existingPortfolio.Positions.Add(position);
                }
            }

            await _context.SaveChangesAsync();

            // Reload to get fresh data
            return await GetPortfolioAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating portfolio");
            throw;
        }
    }

    public async Task<Portfolio> ImportFromCsvAsync(Stream csvStream)
    {
        // TODO: Implement CSV import
        _logger.LogWarning("CSV import not yet implemented");
        throw new NotImplementedException("CSV import functionality is not yet implemented");
    }

    private async Task EnsureSecurityExistsAsync(Security security)
    {
        var existingSecurity = await _context.Securities
            .Include(s => s.Fundamentals)
            .FirstOrDefaultAsync(s => s.Symbol == security.Symbol);

        if (existingSecurity == null)
        {
            _context.Securities.Add(security);
        }
        else
        {
            // Update existing security data
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
                    UpdateFundamentalData(existingSecurity.Fundamentals, security.Fundamentals);
                }
            }
        }
    }

    private void UpdateFundamentalData(FundamentalData target, FundamentalData source)
    {
        target.MarketCap = source.MarketCap;
        target.Revenue = source.Revenue;
        target.FreeCashFlow = source.FreeCashFlow;
        target.RevenueGrowth = source.RevenueGrowth;
        target.EarningsGrowth = source.EarningsGrowth;
        target.DividendYield = source.DividendYield;
        target.DividendPerShare = source.DividendPerShare;
        target.PE = source.PE;
        target.PB = source.PB;
        target.ROE = source.ROE;
        target.DebtToEquity = source.DebtToEquity;
        target.LastUpdated = DateTime.UtcNow;
    }

    private async Task<Portfolio> CreateDefaultPortfolioAsync()
    {
        var portfolio = new Portfolio
        {
            Id = Guid.NewGuid().ToString(),
            Name = "My Portfolio",
            Cash = 0,
            LastUpdated = DateTime.UtcNow,
            Positions = new List<Position>()
        };

        _context.Portfolios.Add(portfolio);
        await _context.SaveChangesAsync();

        return portfolio;
    }
}
