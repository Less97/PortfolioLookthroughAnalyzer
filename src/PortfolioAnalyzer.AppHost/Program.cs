var builder = DistributedApplication.CreateBuilder(args);

// Add PostgreSQL database
var postgres = builder.AddPostgres("postgres")
    .WithDataVolume()
    .WithPgAdmin();

var portfolioDB = postgres.AddDatabase("portfoliodb");

// Add the API service with database reference
var apiService = builder.AddProject<Projects.PortfolioAnalyzer_Api>("apiservice")
    .WithHttpsEndpoint(port: 7000, name: "https")
    .WithHttpEndpoint(port: 5000, name: "http")
    .WithReference(portfolioDB);

// Add the Blazor WebAssembly frontend
builder.AddProject<Projects.PortfolioAnalyzer_Web>("webfrontend")
    .WithHttpsEndpoint(port: 7001, name: "https")
    .WithHttpEndpoint(port: 5001, name: "http")
    .WithReference(apiService);

builder.Build().Run();
