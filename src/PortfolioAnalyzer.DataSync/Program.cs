using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PortfolioAnalyzer.DataSync.Data;
using PortfolioAnalyzer.DataSync.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((context, services) =>
    {
        // Add Application Insights telemetry
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        // Register PostgreSQL DbContext
        var connectionString = context.Configuration["PostgresConnectionString"]
            ?? throw new InvalidOperationException("PostgresConnectionString not configured");

        services.AddDbContext<PortfolioDbContext>(options =>
            options.UseNpgsql(connectionString));

        // Register HttpClient for FMP API
        services.AddHttpClient<FmpApiService>();

        // Register FMP API service
        services.AddScoped<FmpApiService>();
    })
    .Build();

host.Run();
