using PortfolioAnalyzer.Shared.Interfaces;
using PortfolioAnalyzer.Shared.Models;

namespace PortfolioAnalyzer.Api.Services;

public class MockPortfolioService : IPortfolioService
{
    private Portfolio _portfolio;

    public MockPortfolioService()
    {
        _portfolio = CreateMockPortfolio();
    }

    public Task<Portfolio> GetPortfolioAsync()
    {
        return Task.FromResult(_portfolio);
    }

    public Task<Portfolio> UpdatePortfolioAsync(Portfolio portfolio)
    {
        _portfolio = portfolio;
        return Task.FromResult(_portfolio);
    }

    public Task<Portfolio> ImportFromCsvAsync(Stream csvStream)
    {
        // TODO: Implement CSV import
        throw new NotImplementedException("CSV import will be implemented in a future update");
    }

    private static Portfolio CreateMockPortfolio()
    {
        var portfolio = new Portfolio
        {
            Name = "Sample Investment Portfolio",
            Cash = 155000m,
            LastUpdated = DateTime.UtcNow
        };

        portfolio.Positions = new List<Position>
        {
            new Position
            {
                Symbol = "AAPL",
                Quantity = 600m,
                AverageCost = 145.50m,
                CurrentPrice = 178.25m,
                PurchaseDate = DateTime.UtcNow.AddMonths(-18),
                Security = new Security
                {
                    Symbol = "AAPL",
                    Name = "Apple Inc.",
                    Isin = "US0378331005",
                    Sector = "Technology",
                    Industry = "Consumer Electronics",
                    Exchange = "NASDAQ",
                    Currency = "USD",
                    Fundamentals = new FundamentalData
                    {
                        Symbol = "AAPL",
                        MarketCap = 2800000000000m,
                        Revenue = 383285000000m,
                        FreeCashFlow = 99584000000m,
                        RevenueGrowth = 7.8m,
                        EarningsGrowth = 13.4m,
                        DividendYield = 0.52m,
                        DividendPerShare = 0.96m,
                        PE = 28.5m,
                        PB = 45.2m,
                        ROE = 160.5m,
                        DebtToEquity = 181.3m,
                        LastUpdated = DateTime.UtcNow
                    }
                }
            },
            new Position
            {
                Symbol = "MSFT",
                Quantity = 380m,
                AverageCost = 310.20m,
                CurrentPrice = 378.50m,
                PurchaseDate = DateTime.UtcNow.AddMonths(-24),
                Security = new Security
                {
                    Symbol = "MSFT",
                    Name = "Microsoft Corporation",
                    Isin = "US5949181045",
                    Sector = "Technology",
                    Industry = "Software",
                    Exchange = "NASDAQ",
                    Currency = "USD",
                    Fundamentals = new FundamentalData
                    {
                        Symbol = "MSFT",
                        MarketCap = 2810000000000m,
                        Revenue = 211915000000m,
                        FreeCashFlow = 71066000000m,
                        RevenueGrowth = 15.7m,
                        EarningsGrowth = 22.1m,
                        DividendYield = 0.78m,
                        DividendPerShare = 3.00m,
                        PE = 35.8m,
                        PB = 12.5m,
                        ROE = 42.8m,
                        DebtToEquity = 52.1m,
                        LastUpdated = DateTime.UtcNow
                    }
                }
            },
            new Position
            {
                Symbol = "JNJ",
                Quantity = 280m,
                AverageCost = 162.30m,
                CurrentPrice = 155.75m,
                PurchaseDate = DateTime.UtcNow.AddMonths(-12),
                Security = new Security
                {
                    Symbol = "JNJ",
                    Name = "Johnson & Johnson",
                    Isin = "US4781601046",
                    Sector = "Healthcare",
                    Industry = "Pharmaceuticals",
                    Exchange = "NYSE",
                    Currency = "USD",
                    Fundamentals = new FundamentalData
                    {
                        Symbol = "JNJ",
                        MarketCap = 382000000000m,
                        Revenue = 85159000000m,
                        FreeCashFlow = 18500000000m,
                        RevenueGrowth = 5.3m,
                        EarningsGrowth = 8.7m,
                        DividendYield = 3.12m,
                        DividendPerShare = 4.76m,
                        PE = 24.3m,
                        PB = 5.8m,
                        ROE = 25.6m,
                        DebtToEquity = 45.7m,
                        LastUpdated = DateTime.UtcNow
                    }
                }
            },
            new Position
            {
                Symbol = "V",
                Quantity = 250m,
                AverageCost = 225.80m,
                CurrentPrice = 265.40m,
                PurchaseDate = DateTime.UtcNow.AddMonths(-15),
                Security = new Security
                {
                    Symbol = "V",
                    Name = "Visa Inc.",
                    Isin = "US92826C8394",
                    Sector = "Financial Services",
                    Industry = "Credit Services",
                    Exchange = "NYSE",
                    Currency = "USD",
                    Fundamentals = new FundamentalData
                    {
                        Symbol = "V",
                        MarketCap = 545000000000m,
                        Revenue = 32653000000m,
                        FreeCashFlow = 18200000000m,
                        RevenueGrowth = 11.4m,
                        EarningsGrowth = 17.8m,
                        DividendYield = 0.72m,
                        DividendPerShare = 1.94m,
                        PE = 32.1m,
                        PB = 13.2m,
                        ROE = 45.3m,
                        DebtToEquity = 65.8m,
                        LastUpdated = DateTime.UtcNow
                    }
                }
            },
            new Position
            {
                Symbol = "PG",
                Quantity = 280m,
                AverageCost = 142.60m,
                CurrentPrice = 158.30m,
                PurchaseDate = DateTime.UtcNow.AddMonths(-20),
                Security = new Security
                {
                    Symbol = "PG",
                    Name = "Procter & Gamble Co.",
                    Isin = "US7427181091",
                    Sector = "Consumer Defensive",
                    Industry = "Household Products",
                    Exchange = "NYSE",
                    Currency = "USD",
                    Fundamentals = new FundamentalData
                    {
                        Symbol = "PG",
                        MarketCap = 378000000000m,
                        Revenue = 82006000000m,
                        FreeCashFlow = 15800000000m,
                        RevenueGrowth = 3.2m,
                        EarningsGrowth = 6.5m,
                        DividendYield = 2.45m,
                        DividendPerShare = 3.89m,
                        PE = 25.7m,
                        PB = 7.9m,
                        ROE = 32.1m,
                        DebtToEquity = 51.2m,
                        LastUpdated = DateTime.UtcNow
                    }
                }
            },
            new Position
            {
                Symbol = "GOOGL",
                Quantity = 400m,
                AverageCost = 128.40m,
                CurrentPrice = 142.80m,
                PurchaseDate = DateTime.UtcNow.AddMonths(-10),
                Security = new Security
                {
                    Symbol = "GOOGL",
                    Name = "Alphabet Inc.",
                    Isin = "US02079K3059",
                    Sector = "Technology",
                    Industry = "Internet Services",
                    Exchange = "NASDAQ",
                    Currency = "USD",
                    Fundamentals = new FundamentalData
                    {
                        Symbol = "GOOGL",
                        MarketCap = 1780000000000m,
                        Revenue = 307394000000m,
                        FreeCashFlow = 69495000000m,
                        RevenueGrowth = 8.7m,
                        EarningsGrowth = 15.2m,
                        DividendYield = 0.0m,
                        DividendPerShare = 0.0m,
                        PE = 26.3m,
                        PB = 6.2m,
                        ROE = 28.4m,
                        DebtToEquity = 12.8m,
                        LastUpdated = DateTime.UtcNow
                    }
                }
            },
            new Position
            {
                Symbol = "XOM",
                Quantity = 380m,
                AverageCost = 98.50m,
                CurrentPrice = 103.25m,
                PurchaseDate = DateTime.UtcNow.AddMonths(-8),
                Security = new Security
                {
                    Symbol = "XOM",
                    Name = "Exxon Mobil Corporation",
                    Isin = "US30231G1022",
                    Sector = "Energy",
                    Industry = "Oil & Gas",
                    Exchange = "NYSE",
                    Currency = "USD",
                    Fundamentals = new FundamentalData
                    {
                        Symbol = "XOM",
                        MarketCap = 420000000000m,
                        Revenue = 334704000000m,
                        FreeCashFlow = 55200000000m,
                        RevenueGrowth = -2.3m,
                        EarningsGrowth = -35.2m,
                        DividendYield = 3.48m,
                        DividendPerShare = 3.64m,
                        PE = 11.2m,
                        PB = 2.1m,
                        ROE = 19.8m,
                        DebtToEquity = 23.4m,
                        LastUpdated = DateTime.UtcNow
                    }
                }
            },
            new Position
            {
                Symbol = "NVDA",
                Quantity = 180m,
                AverageCost = 425.30m,
                CurrentPrice = 495.20m,
                PurchaseDate = DateTime.UtcNow.AddMonths(-6),
                Security = new Security
                {
                    Symbol = "NVDA",
                    Name = "NVIDIA Corporation",
                    Isin = "US67066G1040",
                    Sector = "Technology",
                    Industry = "Semiconductors",
                    Exchange = "NASDAQ",
                    Currency = "USD",
                    Fundamentals = new FundamentalData
                    {
                        Symbol = "NVDA",
                        MarketCap = 1220000000000m,
                        Revenue = 60922000000m,
                        FreeCashFlow = 27020000000m,
                        RevenueGrowth = 125.9m,
                        EarningsGrowth = 288.5m,
                        DividendYield = 0.03m,
                        DividendPerShare = 0.16m,
                        PE = 68.2m,
                        PB = 52.8m,
                        ROE = 98.3m,
                        DebtToEquity = 35.6m,
                        LastUpdated = DateTime.UtcNow
                    }
                }
            },
            new Position
            {
                Symbol = "AMZN",
                Quantity = 350m,
                AverageCost = 145.80m,
                CurrentPrice = 178.50m,
                PurchaseDate = DateTime.UtcNow.AddMonths(-14),
                Security = new Security
                {
                    Symbol = "AMZN",
                    Name = "Amazon.com Inc.",
                    Isin = "US0231351067",
                    Sector = "Consumer Cyclical",
                    Industry = "Internet Retail",
                    Exchange = "NASDAQ",
                    Currency = "USD",
                    Fundamentals = new FundamentalData
                    {
                        Symbol = "AMZN",
                        MarketCap = 1850000000000m,
                        Revenue = 574785000000m,
                        FreeCashFlow = 35574000000m,
                        RevenueGrowth = 11.8m,
                        EarningsGrowth = 52.3m,
                        DividendYield = 0.0m,
                        DividendPerShare = 0.0m,
                        PE = 62.4m,
                        PB = 8.9m,
                        ROE = 18.7m,
                        DebtToEquity = 58.3m,
                        LastUpdated = DateTime.UtcNow
                    }
                }
            },
            new Position
            {
                Symbol = "JPM",
                Quantity = 380m,
                AverageCost = 142.10m,
                CurrentPrice = 169.75m,
                PurchaseDate = DateTime.UtcNow.AddMonths(-16),
                Security = new Security
                {
                    Symbol = "JPM",
                    Name = "JPMorgan Chase & Co.",
                    Isin = "US46625H1005",
                    Sector = "Financial Services",
                    Industry = "Banks",
                    Exchange = "NYSE",
                    Currency = "USD",
                    Fundamentals = new FundamentalData
                    {
                        Symbol = "JPM",
                        MarketCap = 488000000000m,
                        Revenue = 162421000000m,
                        FreeCashFlow = 48500000000m,
                        RevenueGrowth = 22.1m,
                        EarningsGrowth = 31.6m,
                        DividendYield = 2.35m,
                        DividendPerShare = 4.00m,
                        PE = 11.8m,
                        PB = 1.7m,
                        ROE = 16.5m,
                        DebtToEquity = 125.8m,
                        LastUpdated = DateTime.UtcNow
                    }
                }
            },
            new Position
            {
                Symbol = "TSLA",
                Quantity = 280m,
                AverageCost = 185.40m,
                CurrentPrice = 241.20m,
                PurchaseDate = DateTime.UtcNow.AddMonths(-9),
                Security = new Security
                {
                    Symbol = "TSLA",
                    Name = "Tesla Inc.",
                    Isin = "US88160R1014",
                    Sector = "Consumer Cyclical",
                    Industry = "Auto Manufacturers",
                    Exchange = "NASDAQ",
                    Currency = "USD",
                    Fundamentals = new FundamentalData
                    {
                        Symbol = "TSLA",
                        MarketCap = 767000000000m,
                        Revenue = 96773000000m,
                        FreeCashFlow = 4381000000m,
                        RevenueGrowth = 18.8m,
                        EarningsGrowth = 19.3m,
                        DividendYield = 0.0m,
                        DividendPerShare = 0.0m,
                        PE = 73.5m,
                        PB = 14.2m,
                        ROE = 23.5m,
                        DebtToEquity = 15.2m,
                        LastUpdated = DateTime.UtcNow
                    }
                }
            },
            new Position
            {
                Symbol = "META",
                Quantity = 260m,
                AverageCost = 295.60m,
                CurrentPrice = 485.30m,
                PurchaseDate = DateTime.UtcNow.AddMonths(-22),
                Security = new Security
                {
                    Symbol = "META",
                    Name = "Meta Platforms Inc.",
                    Isin = "US30303M1027",
                    Sector = "Technology",
                    Industry = "Internet Content",
                    Exchange = "NASDAQ",
                    Currency = "USD",
                    Fundamentals = new FundamentalData
                    {
                        Symbol = "META",
                        MarketCap = 1240000000000m,
                        Revenue = 134902000000m,
                        FreeCashFlow = 43000000000m,
                        RevenueGrowth = 23.2m,
                        EarningsGrowth = 73.4m,
                        DividendYield = 0.34m,
                        DividendPerShare = 1.68m,
                        PE = 28.9m,
                        PB = 8.1m,
                        ROE = 32.6m,
                        DebtToEquity = 8.5m,
                        LastUpdated = DateTime.UtcNow
                    }
                }
            }
        };

        return portfolio;
    }
}
