using Microsoft.EntityFrameworkCore;
using PortfolioAnalyzer.Shared.Models;

namespace PortfolioAnalyzer.DataSync.Data;

public class PortfolioDbContext : DbContext
{
    public PortfolioDbContext(DbContextOptions<PortfolioDbContext> options)
        : base(options)
    {
    }

    public DbSet<Portfolio> Portfolios => Set<Portfolio>();
    public DbSet<Position> Positions => Set<Position>();
    public DbSet<Security> Securities => Set<Security>();
    public DbSet<FundamentalData> FundamentalData => Set<FundamentalData>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Portfolio configuration
        modelBuilder.Entity<Portfolio>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).IsRequired();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Cash).HasPrecision(18, 2);
            entity.Property(e => e.LastUpdated).IsRequired();

            // Owned collection for positions
            entity.HasMany(e => e.Positions)
                .WithOne()
                .HasForeignKey("PortfolioId")
                .OnDelete(DeleteBehavior.Cascade);

            // Ignore calculated properties
            entity.Ignore(e => e.TotalMarketValue);
            entity.Ignore(e => e.TotalCostBasis);
            entity.Ignore(e => e.TotalUnrealizedPnL);
            entity.Ignore(e => e.TotalUnrealizedPnLPercent);
        });

        // Position configuration
        modelBuilder.Entity<Position>(entity =>
        {
            // Shadow property for primary key
            entity.Property<int>("Id");
            entity.HasKey("Id");

            // Shadow property for foreign key
            entity.Property<string>("PortfolioId").IsRequired();

            entity.Property(e => e.Symbol).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Quantity).HasPrecision(18, 8);
            entity.Property(e => e.AverageCost).HasPrecision(18, 4);
            entity.Property(e => e.CurrentPrice).HasPrecision(18, 4);
            entity.Property(e => e.PurchaseDate).IsRequired();

            // Relationship with Security
            entity.HasOne(e => e.Security)
                .WithMany()
                .HasForeignKey(e => e.Symbol)
                .HasPrincipalKey(s => s.Symbol);

            // Ignore calculated properties
            entity.Ignore(e => e.MarketValue);
            entity.Ignore(e => e.CostBasis);
            entity.Ignore(e => e.UnrealizedPnL);
            entity.Ignore(e => e.UnrealizedPnLPercent);
        });

        // Security configuration
        modelBuilder.Entity<Security>(entity =>
        {
            entity.HasKey(e => e.Symbol);
            entity.Property(e => e.Symbol).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Isin).HasMaxLength(12);
            entity.Property(e => e.Sector).HasMaxLength(100);
            entity.Property(e => e.Industry).HasMaxLength(100);
            entity.Property(e => e.Exchange).HasMaxLength(50);
            entity.Property(e => e.Currency).HasMaxLength(3).IsRequired();

            // One-to-one relationship with FundamentalData
            entity.HasOne(e => e.Fundamentals)
                .WithOne()
                .HasForeignKey<FundamentalData>(f => f.Symbol)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // FundamentalData configuration
        modelBuilder.Entity<FundamentalData>(entity =>
        {
            entity.HasKey(e => e.Symbol);
            entity.Property(e => e.Symbol).IsRequired().HasMaxLength(20);
            entity.Property(e => e.MarketCap).HasPrecision(20, 2);
            entity.Property(e => e.Revenue).HasPrecision(20, 2);
            entity.Property(e => e.FreeCashFlow).HasPrecision(20, 2);
            entity.Property(e => e.RevenueGrowth).HasPrecision(8, 4);
            entity.Property(e => e.EarningsGrowth).HasPrecision(8, 4);
            entity.Property(e => e.DividendYield).HasPrecision(8, 4);
            entity.Property(e => e.DividendPerShare).HasPrecision(18, 4);
            entity.Property(e => e.PE).HasPrecision(10, 2);
            entity.Property(e => e.PB).HasPrecision(10, 2);
            entity.Property(e => e.ROE).HasPrecision(8, 4);
            entity.Property(e => e.DebtToEquity).HasPrecision(10, 2);
            entity.Property(e => e.LastUpdated).IsRequired();
        });
    }
}
