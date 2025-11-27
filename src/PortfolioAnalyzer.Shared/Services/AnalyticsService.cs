using PortfolioAnalyzer.Shared.Interfaces;
using PortfolioAnalyzer.Shared.Models;

namespace PortfolioAnalyzer.Shared.Services;

public class AnalyticsService : IAnalyticsService
{
    public Task<PortfolioAnalytics> CalculateAnalyticsAsync(Portfolio portfolio)
    {
        var analytics = new PortfolioAnalytics();
        var totalPortfolioValue = portfolio.TotalMarketValue;

        if (portfolio.Positions.Count == 0 || totalPortfolioValue == 0)
        {
            return Task.FromResult(analytics);
        }

        // Calculate weighted fundamentals
        decimal totalRevenue = 0;
        decimal totalFreeCashFlow = 0;
        decimal weightedRevenueGrowth = 0;
        decimal weightedEarningsGrowth = 0;
        decimal totalAnnualDividends = 0;

        var sectorGroups = new Dictionary<string, SectorAllocation>();
        var contributions = new List<PositionContribution>();
        var dividendContributions = new List<DividendContribution>();

        foreach (var position in portfolio.Positions)
        {
            var positionValue = position.MarketValue;
            var weight = positionValue / totalPortfolioValue;

            if (position.Security?.Fundamentals != null)
            {
                var fundamentals = position.Security.Fundamentals;

                // Look-through revenue and FCF: (Company Metric / Market Cap) × Position Value
                // This calculates your proportional share of the company's fundamentals
                if (fundamentals.MarketCap > 0)
                {
                    totalRevenue += (fundamentals.Revenue / fundamentals.MarketCap) * positionValue;
                    totalFreeCashFlow += (fundamentals.FreeCashFlow / fundamentals.MarketCap) * positionValue;
                }

                // Weighted growth rates
                weightedRevenueGrowth += fundamentals.RevenueGrowth * weight;
                weightedEarningsGrowth += fundamentals.EarningsGrowth * weight;

                // Dividend calculations
                var positionAnnualDividend = fundamentals.DividendPerShare * position.Quantity;
                totalAnnualDividends += positionAnnualDividend;

                if (positionAnnualDividend > 0)
                {
                    dividendContributions.Add(new DividendContribution
                    {
                        Symbol = position.Symbol,
                        Name = position.Security.Name,
                        AnnualDividend = positionAnnualDividend,
                        Yield = fundamentals.DividendYield,
                        Percentage = 0 // Will calculate after we have total
                    });
                }
            }

            // Sector allocation
            var sector = position.Security?.Sector ?? "Unknown";
            if (!sectorGroups.ContainsKey(sector))
            {
                sectorGroups[sector] = new SectorAllocation
                {
                    Sector = sector,
                    Value = 0,
                    PositionCount = 0
                };
            }
            sectorGroups[sector].Value += positionValue;
            sectorGroups[sector].PositionCount++;

            // Position contribution
            contributions.Add(new PositionContribution
            {
                Symbol = position.Symbol,
                Name = position.Security?.Name ?? position.Symbol,
                Value = positionValue,
                ContributionAmount = position.UnrealizedPnL,
                ContributionPercent = position.UnrealizedPnLPercent,
                Weight = weight * 100
            });
        }

        // Calculate sector percentages
        foreach (var sector in sectorGroups.Values)
        {
            sector.Percentage = (sector.Value / totalPortfolioValue) * 100;
        }

        // Calculate dividend percentages
        if (totalAnnualDividends > 0)
        {
            foreach (var dividend in dividendContributions)
            {
                dividend.Percentage = (dividend.AnnualDividend / totalAnnualDividends) * 100;
            }
        }

        // Sort contributors and detractors
        var topContributors = contributions
            .Where(c => c.ContributionAmount > 0)
            .OrderByDescending(c => c.ContributionAmount)
            .Take(10)
            .ToList();

        var topDetractors = contributions
            .Where(c => c.ContributionAmount < 0)
            .OrderBy(c => c.ContributionAmount)
            .Take(10)
            .ToList();

        analytics.TotalRevenue = totalRevenue;
        analytics.TotalFreeCashFlow = totalFreeCashFlow;
        analytics.WeightedRevenueGrowth = weightedRevenueGrowth;
        analytics.WeightedEarningsGrowth = weightedEarningsGrowth;
        analytics.TotalAnnualDividends = totalAnnualDividends;
        analytics.TotalDividendYield = totalPortfolioValue > 0
            ? (totalAnnualDividends / totalPortfolioValue) * 100
            : 0;

        analytics.SectorAllocations = sectorGroups.Values.OrderByDescending(s => s.Value).ToList();
        analytics.TopContributors = topContributors;
        analytics.TopDetractors = topDetractors;
        analytics.DividendBreakdown = dividendContributions.OrderByDescending(d => d.AnnualDividend).ToList();

        return Task.FromResult(analytics);
    }
}
