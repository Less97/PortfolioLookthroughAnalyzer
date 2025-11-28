# Portfolio Lookthrough Analyzer

A comprehensive portfolio analysis tool built with .NET 8, Blazor WebAssembly, PostgreSQL, and .NET Aspire. Analyze your investment portfolio with detailed fundamental data, sector allocation, performance attribution, and dividend tracking.

Inspired by Warren Buffett's "look-through earnings" philosophy - view your portfolio as ownership stakes in underlying businesses, not just fluctuating stock prices.

## Features

### 📊 Portfolio Analytics
- **Fundamental Analysis**: View aggregated revenue, free cash flow, and growth metrics weighted by position size
- **Performance Attribution**: Track top contributors and detractors to portfolio performance
- **Sector Allocation**: Visual breakdown of portfolio by sector with interactive charts
- **Dividend Tracking**: Analyze dividend income by position and overall yield

### 📈 Visualizations
- Interactive charts using Radzen Blazor Components
- Sector allocation pie charts
- Performance attribution bar charts
- Dividend contribution analysis with data grids
- Real-time portfolio metrics

### 🔧 Technical Features
- **Blazor WebAssembly**: Fast, client-side SPA with C#
- **PostgreSQL Database**: Persistent data storage via .NET Aspire
- **Azure Functions**: Automated data sync with timer triggers
- **Financial Modeling Prep API**: Professional-grade fundamental data
- **MAUI-Ready**: Shared class library compatible with future MAUI mobile app
- **.NET Aspire**: Modern cloud-native orchestration with PostgreSQL and pgAdmin
- **GitHub Actions CI/CD**: Automated deployment workflows

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [.NET Aspire workload](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/setup-tooling)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) - Required for PostgreSQL container
- Visual Studio 2022 17.9+ or Visual Studio Code with C# Dev Kit
- [Financial Modeling Prep API Key](https://site.financialmodelingprep.com/developer/docs) - Starter plan ~$15/month (free tier available)

## Installation

### Install .NET Aspire

```bash
dotnet workload update
dotnet workload install aspire
```

### Clone and Build

```bash
git clone <your-repo-url>
cd PortfolioLookthrouhAnalyzer
dotnet restore
dotnet build
```

## Configuration

### Financial Modeling Prep API Key

1. Sign up for an API key at [Financial Modeling Prep](https://site.financialmodelingprep.com/developer/docs)
2. Add your API key to `src/PortfolioAnalyzer.DataSync/local.settings.json`:

```json
{
  "Values": {
    "FmpApiKey": "YOUR_FMP_API_KEY_HERE",
    "PostgresConnectionString": "Host=localhost;Database=portfoliodb;Username=postgres;Password=postgres"
  }
}
```

### Ticker Configuration

Edit `src/PortfolioAnalyzer.DataSync/tickers.json` to configure which stocks to track:

```json
{
  "tickers": ["AAPL", "MSFT", "GOOGL", "AMZN", "NVDA"],
  "syncSchedule": "0 0 6 * * *",
  "description": "Daily sync at 6:00 AM UTC"
}
```

## Running the Application

### Using .NET Aspire (Recommended)

```bash
cd src/PortfolioAnalyzer.AppHost
dotnet run
```

This will start:
- **PostgreSQL Database**: Automatically provisioned in Docker container
- **pgAdmin**: Web-based database management UI
- **API Service**: https://localhost:7000
- **Blazor WebAssembly**: https://localhost:7001
- **Aspire Dashboard**: https://localhost:15888 (for monitoring)

The database schema is automatically created on first run using Entity Framework Core.

### Running Services Individually

**API:**
```bash
cd src/PortfolioAnalyzer.Api
dotnet run
```

**Blazor WebAssembly:**
```bash
cd src/PortfolioAnalyzer.Web
dotnet run
```

## Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                    .NET Aspire AppHost                      │
│  Orchestrates: API + Web + PostgreSQL + pgAdmin + Redis    │
└─────────────────────────────────────────────────────────────┘
                              │
        ┌─────────────────────┼─────────────────────┐
        ▼                     ▼                     ▼
┌───────────────┐    ┌─────────────────┐   ┌──────────────┐
│  Blazor WASM  │───▶│  ASP.NET Core   │──▶│  PostgreSQL  │
│  Frontend     │    │  REST API       │   │  Database    │
│  (Port 7001)  │    │  (Port 7000)    │   │              │
└───────────────┘    └─────────────────┘   └──────────────┘
                              ▲                     ▲
                              │                     │
                     ┌────────────────────┐         │
                     │  Azure Function    │─────────┘
                     │  Timer Trigger     │
                     │  (Data Sync)       │
                     └────────────────────┘
                              │
                              ▼
                     ┌────────────────────┐
                     │  FMP API           │
                     │  (Financial Data)  │
                     └────────────────────┘
```

## Project Structure

```
PortfolioLookthroughAnalyzer/
├── src/
│   ├── PortfolioAnalyzer.Shared/          # MAUI-compatible shared library
│   │   ├── Models/                         # Domain models (Portfolio, Position, Security)
│   │   ├── Services/                       # Business logic (AnalyticsService)
│   │   └── Interfaces/                     # Service contracts
│   │
│   ├── PortfolioAnalyzer.Api/             # ASP.NET Core Web API
│   │   ├── Controllers/                    # API endpoints
│   │   ├── Services/                       # PostgreSQL service implementation
│   │   └── Data/                           # EF Core DbContext
│   │
│   ├── PortfolioAnalyzer.Web/             # Blazor WebAssembly
│   │   ├── Pages/                          # Razor pages (Dashboard, Holdings, Analytics)
│   │   ├── Components/                     # Reusable Blazor components
│   │   └── Services/                       # API clients
│   │
│   ├── PortfolioAnalyzer.DataSync/        # Azure Function (Timer Trigger)
│   │   ├── Functions/                      # Timer trigger for data sync
│   │   ├── Services/                       # FMP API service
│   │   ├── Data/                           # EF Core DbContext
│   │   └── tickers.json                    # Stocks to track
│   │
│   ├── PortfolioAnalyzer.AppHost/         # .NET Aspire orchestration
│   └── PortfolioAnalyzer.ServiceDefaults/ # Shared Aspire configuration
│
├── .github/workflows/                      # CI/CD pipelines
│   ├── deploy-api-web.yml                 # API & Web deployment
│   └── deploy-datasync-function.yml       # Azure Function deployment
│
└── PortfolioAnalyzer.sln
```

## Roadmap

### Phase 1 (Completed)
- [x] Core domain models (Portfolio, Position, Security, FundamentalData)
- [x] PostgreSQL database with Entity Framework Core
- [x] Analytics engine (Look-through revenue, FCF, earnings, growth, sectors)
- [x] Blazor WebAssembly UI with Radzen charts
- [x] .NET Aspire orchestration with PostgreSQL
- [x] Azure Function Timer Trigger for data sync
- [x] FMP API integration for fundamental data
- [x] GitHub Actions CI/CD workflows

### Phase 2 (Next)
- [ ] Watchlist feature with valuation alerts
- [ ] Advanced metrics (ROCE, ROIC, Piotroski F-Score, Altman Z-Score)
- [ ] Owner earnings calculations (Buffett-style)
- [ ] CSV import functionality for Degiro/Interactive Brokers
- [ ] Historical performance tracking
- [ ] Multiple portfolio support

### Phase 3 (Future)
- [ ] User authentication and cloud sync
- [ ] MAUI mobile application
- [ ] What-if scenario modeling
- [ ] Portfolio comparison to index fundamentals
- [ ] Tax loss harvesting suggestions
- [ ] Export/reporting (PDF summaries)

## Deployment

### Azure Deployment

The application is designed for deployment to Azure using the free tier resources:

#### Prerequisites
1. **Azure Account** - [Create free account](https://azure.microsoft.com/free/)
2. **Azure CLI** - [Install Azure CLI](https://docs.microsoft.com/cli/azure/install-azure-cli)
3. **GitHub Secrets** - Configure in repository settings:
   - `AZURE_API_APP_NAME` - App Service name for API
   - `AZURE_API_PUBLISH_PROFILE` - Download from Azure Portal
   - `AZURE_FUNCTION_APP_NAME` - Function App name
   - `AZURE_FUNCTION_PUBLISH_PROFILE` - Download from Azure Portal
   - `AZURE_STATIC_WEB_APPS_API_TOKEN` - Auto-generated when creating Static Web App
   - `FMP_API_KEY` - Financial Modeling Prep API key
   - `POSTGRES_CONNECTION_STRING` - Neon PostgreSQL connection string

#### Azure Resources Required

| Resource | Service | Tier | Cost |
|----------|---------|------|------|
| API Backend | Azure App Service | Free (F1) | £0 |
| Blazor Frontend | Azure Static Web Apps | Free | £0 |
| Data Sync | Azure Functions | Consumption | £0* |
| Database | Neon PostgreSQL | Free (512MB) | £0 |

*Azure Functions Consumption plan includes 1M free executions/month

#### Deployment Steps

1. **Create Azure Resources**:
```bash
# Login to Azure
az login

# Create resource group
az group create --name portfolio-analyzer-rg --location eastus

# Create App Service for API
az webapp up --name portfolio-analyzer-api --resource-group portfolio-analyzer-rg --runtime "DOTNETCORE:8.0"

# Create Azure Function App
az functionapp create --name portfolio-analyzer-sync --resource-group portfolio-analyzer-rg --consumption-plan-location eastus --runtime dotnet-isolated --runtime-version 8 --storage-account portfolioanalyzerstorage

# Create Static Web App (use GitHub integration via Azure Portal)
```

2. **Configure Connection Strings**:
```bash
# Set PostgreSQL connection string in App Service
az webapp config connection-string set --name portfolio-analyzer-api --resource-group portfolio-analyzer-rg --settings portfoliodb="YOUR_POSTGRES_CONNECTION_STRING" --connection-string-type PostgreSQL

# Set FMP API key in Function App
az functionapp config appsettings set --name portfolio-analyzer-sync --resource-group portfolio-analyzer-rg --settings FmpApiKey="YOUR_FMP_API_KEY" PostgresConnectionString="YOUR_POSTGRES_CONNECTION_STRING"
```

3. **Deploy via GitHub Actions**:

Push to `main` branch to trigger automatic deployment via GitHub Actions workflows.

### Local Development with PostgreSQL

Using Neon PostgreSQL (free tier):
1. Create free database at [neon.tech](https://neon.tech)
2. Update connection string in `local.settings.json`
3. Run migrations: `dotnet ef database update`

## Technologies Used

### Frontend
- **.NET 8**: Latest LTS version of .NET
- **Blazor WebAssembly**: Client-side C# framework
- **Radzen Blazor**: Beautiful, interactive charts and data grids

### Backend
- **ASP.NET Core**: REST API with Entity Framework Core
- **PostgreSQL**: Relational database (via .NET Aspire)
- **Azure Functions**: Serverless timer triggers for data sync

### Data & APIs
- **Financial Modeling Prep**: Professional-grade fundamental data API
- **Entity Framework Core**: ORM with Npgsql provider

### DevOps & Infrastructure
- **.NET Aspire**: Cloud-native orchestration with PostgreSQL, pgAdmin, Redis
- **GitHub Actions**: CI/CD pipelines for automated deployment
- **Docker**: PostgreSQL containerization (local development)
- **Azure**: Cloud hosting (App Service, Static Web Apps, Functions)

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License.

## Support

For issues or questions, please open an issue on GitHub.

---

Built with ❤️ using .NET and Blazor
