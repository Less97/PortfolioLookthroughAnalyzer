using PortfolioAnalyzer.Api.Data;
using PortfolioAnalyzer.Api.Services;
using PortfolioAnalyzer.Shared.Interfaces;
using PortfolioAnalyzer.Shared.Services;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components
builder.AddServiceDefaults();

// Add PostgreSQL database with Aspire
builder.AddNpgsqlDbContext<PortfolioDbContext>("portfoliodb");

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register application services
builder.Services.AddScoped<IPortfolioService, PostgresPortfolioService>();
builder.Services.AddSingleton<IFundamentalDataService, MockFundamentalDataService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

// Configure CORS for Blazor WebAssembly
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorWasm", policy =>
    {
        policy.WithOrigins("https://localhost:7001", "http://localhost:5001")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Apply database migrations automatically
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PortfolioDbContext>();
    dbContext.Database.EnsureCreated(); // For development; use Migrate() in production
}

// Configure the HTTP request pipeline
app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowBlazorWasm");
app.UseAuthorization();
app.MapControllers();

app.Run();
