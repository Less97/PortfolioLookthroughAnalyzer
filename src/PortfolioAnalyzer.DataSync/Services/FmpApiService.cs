using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PortfolioAnalyzer.Shared.Models;

namespace PortfolioAnalyzer.DataSync.Services;

/// <summary>
/// Service for fetching financial data from Financial Modeling Prep (FMP) API
/// API Documentation: https://site.financialmodelingprep.com/developer/docs
/// </summary>
public class FmpApiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<FmpApiService> _logger;
    private readonly string _apiKey;
    private readonly string _baseUrl;

    public FmpApiService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<FmpApiService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
        _apiKey = _configuration["FmpApiKey"] ?? throw new InvalidOperationException("FmpApiKey not configured");
        _baseUrl = _configuration["FmpApiBaseUrl"] ?? "https://financialmodelingprep.com/api/v3";
    }

    public async Task<Security?> GetSecurityDataAsync(string ticker)
    {
        try
        {
            _logger.LogInformation("Fetching data for {Ticker} from FMP API", ticker);

            // Fetch company profile
            var profile = await GetCompanyProfileAsync(ticker);
            if (profile == null)
            {
                _logger.LogWarning("No profile data found for {Ticker}", ticker);
                return null;
            }

            // Fetch key metrics
            var metrics = await GetKeyMetricsAsync(ticker);
            var ratios = await GetFinancialRatiosAsync(ticker);
            var quote = await GetQuoteAsync(ticker);

            var security = new Security
            {
                Symbol = ticker,
                Name = profile.CompanyName ?? ticker,
                Isin = profile.Isin ?? string.Empty,
                Sector = profile.Sector ?? string.Empty,
                Industry = profile.Industry ?? string.Empty,
                Exchange = profile.Exchange ?? string.Empty,
                Currency = profile.Currency ?? "USD",
                Fundamentals = new FundamentalData
                {
                    Symbol = ticker,
                    MarketCap = quote?.MarketCap ?? profile.MktCap ?? 0,
                    Revenue = metrics?.Revenue ?? 0,
                    FreeCashFlow = metrics?.FreeCashFlow ?? 0,
                    RevenueGrowth = ratios?.RevenueGrowth ?? 0,
                    EarningsGrowth = metrics?.NetIncomeGrowth ?? 0,
                    DividendYield = quote?.LastAnnualDividend > 0 && quote?.Price > 0
                        ? (quote.LastAnnualDividend / quote.Price) * 100
                        : 0,
                    DividendPerShare = quote?.LastAnnualDividend ?? 0,
                    PE = quote?.Pe ?? 0,
                    PB = ratios?.PriceToBookRatio ?? 0,
                    ROE = ratios?.ReturnOnEquity ?? 0,
                    DebtToEquity = ratios?.DebtEquityRatio ?? 0,
                    LastUpdated = DateTime.UtcNow
                }
            };

            _logger.LogInformation("Successfully fetched data for {Ticker}", ticker);
            return security;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching data for {Ticker}", ticker);
            return null;
        }
    }

    private async Task<FmpCompanyProfile?> GetCompanyProfileAsync(string ticker)
    {
        var url = $"{_baseUrl}/profile/{ticker}?apikey={_apiKey}";
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Failed to fetch profile for {Ticker}: {StatusCode}", ticker, response.StatusCode);
            return null;
        }

        var json = await response.Content.ReadAsStringAsync();
        var profiles = JsonSerializer.Deserialize<List<FmpCompanyProfile>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return profiles?.FirstOrDefault();
    }

    private async Task<FmpKeyMetrics?> GetKeyMetricsAsync(string ticker)
    {
        var url = $"{_baseUrl}/key-metrics-ttm/{ticker}?apikey={_apiKey}";
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Failed to fetch key metrics for {Ticker}: {StatusCode}", ticker, response.StatusCode);
            return null;
        }

        var json = await response.Content.ReadAsStringAsync();
        var metrics = JsonSerializer.Deserialize<List<FmpKeyMetrics>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return metrics?.FirstOrDefault();
    }

    private async Task<FmpFinancialRatios?> GetFinancialRatiosAsync(string ticker)
    {
        var url = $"{_baseUrl}/ratios-ttm/{ticker}?apikey={_apiKey}";
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Failed to fetch ratios for {Ticker}: {StatusCode}", ticker, response.StatusCode);
            return null;
        }

        var json = await response.Content.ReadAsStringAsync();
        var ratios = JsonSerializer.Deserialize<List<FmpFinancialRatios>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return ratios?.FirstOrDefault();
    }

    private async Task<FmpQuote?> GetQuoteAsync(string ticker)
    {
        var url = $"{_baseUrl}/quote/{ticker}?apikey={_apiKey}";
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Failed to fetch quote for {Ticker}: {StatusCode}", ticker, response.StatusCode);
            return null;
        }

        var json = await response.Content.ReadAsStringAsync();
        var quotes = JsonSerializer.Deserialize<List<FmpQuote>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return quotes?.FirstOrDefault();
    }
}

// FMP API Response Models
public class FmpCompanyProfile
{
    [JsonPropertyName("symbol")]
    public string? Symbol { get; set; }

    [JsonPropertyName("companyName")]
    public string? CompanyName { get; set; }

    [JsonPropertyName("isin")]
    public string? Isin { get; set; }

    [JsonPropertyName("sector")]
    public string? Sector { get; set; }

    [JsonPropertyName("industry")]
    public string? Industry { get; set; }

    [JsonPropertyName("exchangeShortName")]
    public string? Exchange { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    [JsonPropertyName("mktCap")]
    public decimal MktCap { get; set; }
}

public class FmpKeyMetrics
{
    [JsonPropertyName("revenuelPerShareTTM")]
    public decimal Revenue { get; set; }

    [JsonPropertyName("freeCashFlowPerShareTTM")]
    public decimal FreeCashFlow { get; set; }

    [JsonPropertyName("netIncomePerShareTTM")]
    public decimal NetIncome { get; set; }

    [JsonPropertyName("roeTTM")]
    public decimal ROE { get; set; }

    [JsonPropertyName("peRatioTTM")]
    public decimal PE { get; set; }

    [JsonPropertyName("marketCapTTM")]
    public decimal MarketCap { get; set; }

    [JsonPropertyName("dividendYieldTTM")]
    public decimal DividendYield { get; set; }

    [JsonPropertyName("revenueGrowth")]
    public decimal RevenueGrowth { get; set; }

    [JsonPropertyName("netIncomeGrowth")]
    public decimal NetIncomeGrowth { get; set; }
}

public class FmpFinancialRatios
{
    [JsonPropertyName("returnOnEquityTTM")]
    public decimal ReturnOnEquity { get; set; }

    [JsonPropertyName("debtEquityRatioTTM")]
    public decimal DebtEquityRatio { get; set; }

    [JsonPropertyName("priceToBookRatioTTM")]
    public decimal PriceToBookRatio { get; set; }

    [JsonPropertyName("revenueGrowthTTM")]
    public decimal RevenueGrowth { get; set; }
}

public class FmpQuote
{
    [JsonPropertyName("symbol")]
    public string? Symbol { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("pe")]
    public decimal Pe { get; set; }

    [JsonPropertyName("marketCap")]
    public decimal MarketCap { get; set; }

    [JsonPropertyName("lastAnnualDividend")]
    public decimal LastAnnualDividend { get; set; }
}
