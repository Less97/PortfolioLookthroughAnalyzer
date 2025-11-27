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
            Name = "Growth & Income Portfolio",
            Cash = 50000m,
            LastUpdated = DateTime.UtcNow
        };

        // Total Portfolio Value: $1,000,000 ($950,000 invested + $50,000 cash)
        // Diversified across 10 positions with realistic allocations
        portfolio.Positions = new List<Position>
        {
            // Position 1: Apple Inc. - $150,000 (15.0%)
            new Position
            {
                Symbol = "AAPL",
                Quantity = 800m,
                AverageCost = 150.00m,
                CurrentPrice = 187.50m,
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
                        MarketCap = 2900000000000m, // $2.9T
                        Revenue = 383285000000m, // $383.3B
                        FreeCashFlow = 99584000000m, // $99.6B
                        RevenueGrowth = 7.8m,
                        EarningsGrowth = 13.4m,
                        DividendYield = 0.51m,
                        DividendPerShare = 0.96m,
                        PE = 29.2m,
                        PB = 46.5m,
                        ROE = 162.4m,
                        DebtToEquity = 181.7m,
                        LastUpdated = DateTime.UtcNow
                    }
                }
            },

            // Position 2: Microsoft Corporation - $140,000 (14.0%)
            new Position
            {
                Symbol = "MSFT",
                Quantity = 350m,
                AverageCost = 320.00m,
                CurrentPrice = 400.00m,
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
                        MarketCap = 2980000000000m, // $2.98T
                        Revenue = 211915000000m, // $211.9B
                        FreeCashFlow = 71066000000m, // $71.1B
                        RevenueGrowth = 15.7m,
                        EarningsGrowth = 22.1m,
                        DividendYield = 0.75m,
                        DividendPerShare = 3.00m,
                        PE = 36.5m,
                        PB = 13.2m,
                        ROE = 43.5m,
                        DebtToEquity = 52.8m,
                        LastUpdated = DateTime.UtcNow
                    }
                }
            },

            // Position 3: NVIDIA Corporation - $120,000 (12.0%)
            new Position
            {
                Symbol = "NVDA",
                Quantity = 240m,
                AverageCost = 400.00m,
                CurrentPrice = 500.00m,
                PurchaseDate = DateTime.UtcNow.AddMonths(-12),
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
                        MarketCap = 1230000000000m, // $1.23T
                        Revenue = 60922000000m, // $60.9B
                        FreeCashFlow = 27020000000m, // $27.0B
                        RevenueGrowth = 125.9m,
                        EarningsGrowth = 288.5m,
                        DividendYield = 0.03m,
                        DividendPerShare = 0.16m,
                        PE = 70.5m,
                        PB = 55.3m,
                        ROE = 100.2m,
                        DebtToEquity = 36.1m,
                        LastUpdated = DateTime.UtcNow
                    }
                }
            },

            // Position 4: Alphabet Inc. - $100,000 (10.0%)
            new Position
            {
                Symbol = "GOOGL",
                Quantity = 625m,
                AverageCost = 120.00m,
                CurrentPrice = 160.00m,
                PurchaseDate = DateTime.UtcNow.AddMonths(-15),
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
                        MarketCap = 2000000000000m, // $2.0T
                        Revenue = 307394000000m, // $307.4B
                        FreeCashFlow = 69495000000m, // $69.5B
                        RevenueGrowth = 8.7m,
                        EarningsGrowth = 15.2m,
                        DividendYield = 0.0m,
                        DividendPerShare = 0.0m,
                        PE = 27.8m,
                        PB = 6.8m,
                        ROE = 29.1m,
                        DebtToEquity = 13.2m,
                        LastUpdated = DateTime.UtcNow
                    }
                }
            },

            // Position 5: Amazon.com Inc. - $90,000 (9.0%)
            new Position
            {
                Symbol = "AMZN",
                Quantity = 500m,
                AverageCost = 140.00m,
                CurrentPrice = 180.00m,
                PurchaseDate = DateTime.UtcNow.AddMonths(-20),
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
                        MarketCap = 1870000000000m, // $1.87T
                        Revenue = 574785000000m, // $574.8B
                        FreeCashFlow = 35574000000m, // $35.6B
                        RevenueGrowth = 11.8m,
                        EarningsGrowth = 52.3m,
                        DividendYield = 0.0m,
                        DividendPerShare = 0.0m,
                        PE = 64.2m,
                        PB = 9.3m,
                        ROE = 19.2m,
                        DebtToEquity = 59.1m,
                        LastUpdated = DateTime.UtcNow
                    }
                }
            },

            // Position 6: JPMorgan Chase & Co. - $80,000 (8.0%)
            new Position
            {
                Symbol = "JPM",
                Quantity = 400m,
                AverageCost = 160.00m,
                CurrentPrice = 200.00m,
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
                        MarketCap = 575000000000m, // $575B
                        Revenue = 162421000000m, // $162.4B
                        FreeCashFlow = 48500000000m, // $48.5B
                        RevenueGrowth = 22.1m,
                        EarningsGrowth = 31.6m,
                        DividendYield = 2.0m,
                        DividendPerShare = 4.00m,
                        PE = 12.5m,
                        PB = 1.9m,
                        ROE = 17.2m,
                        DebtToEquity = 126.3m,
                        LastUpdated = DateTime.UtcNow
                    }
                }
            },

            // Position 7: Visa Inc. - $70,000 (7.0%)
            new Position
            {
                Symbol = "V",
                Quantity = 250m,
                AverageCost = 240.00m,
                CurrentPrice = 280.00m,
                PurchaseDate = DateTime.UtcNow.AddMonths(-14),
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
                        MarketCap = 575000000000m, // $575B
                        Revenue = 32653000000m, // $32.7B
                        FreeCashFlow = 18200000000m, // $18.2B
                        RevenueGrowth = 11.4m,
                        EarningsGrowth = 17.8m,
                        DividendYield = 0.71m,
                        DividendPerShare = 2.00m,
                        PE = 33.5m,
                        PB = 14.1m,
                        ROE = 46.8m,
                        DebtToEquity = 66.5m,
                        LastUpdated = DateTime.UtcNow
                    }
                }
            },

            // Position 8: Johnson & Johnson - $70,000 (7.0%)
            new Position
            {
                Symbol = "JNJ",
                Quantity = 437.5m,
                AverageCost = 155.00m,
                CurrentPrice = 160.00m,
                PurchaseDate = DateTime.UtcNow.AddMonths(-22),
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
                        MarketCap = 390000000000m, // $390B
                        Revenue = 85159000000m, // $85.2B
                        FreeCashFlow = 18500000000m, // $18.5B
                        RevenueGrowth = 5.3m,
                        EarningsGrowth = 8.7m,
                        DividendYield = 3.06m,
                        DividendPerShare = 4.90m,
                        PE = 25.1m,
                        PB = 6.2m,
                        ROE = 26.3m,
                        DebtToEquity = 46.2m,
                        LastUpdated = DateTime.UtcNow
                    }
                }
            },

            // Position 9: Procter & Gamble Co. - $65,000 (6.5%)
            new Position
            {
                Symbol = "PG",
                Quantity = 400m,
                AverageCost = 145.00m,
                CurrentPrice = 162.50m,
                PurchaseDate = DateTime.UtcNow.AddMonths(-10),
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
                        MarketCap = 385000000000m, // $385B
                        Revenue = 82006000000m, // $82.0B
                        FreeCashFlow = 15800000000m, // $15.8B
                        RevenueGrowth = 3.2m,
                        EarningsGrowth = 6.5m,
                        DividendYield = 2.43m,
                        DividendPerShare = 3.95m,
                        PE = 26.3m,
                        PB = 8.5m,
                        ROE = 33.2m,
                        DebtToEquity = 52.1m,
                        LastUpdated = DateTime.UtcNow
                    }
                }
            },

            // Position 10: Exxon Mobil Corporation - $65,000 (6.5%)
            new Position
            {
                Symbol = "XOM",
                Quantity = 600m,
                AverageCost = 95.00m,
                CurrentPrice = 108.33m,
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
                        MarketCap = 445000000000m, // $445B
                        Revenue = 334704000000m, // $334.7B
                        FreeCashFlow = 55200000000m, // $55.2B
                        RevenueGrowth = -2.3m,
                        EarningsGrowth = -8.5m,
                        DividendYield = 3.31m,
                        DividendPerShare = 3.60m,
                        PE = 11.8m,
                        PB = 2.3m,
                        ROE = 20.5m,
                        DebtToEquity = 24.1m,
                        LastUpdated = DateTime.UtcNow
                    }
                }
            }
        };

        return portfolio;
    }
}
