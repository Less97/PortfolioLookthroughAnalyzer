using Microsoft.AspNetCore.Mvc;
using PortfolioAnalyzer.Shared.Interfaces;
using PortfolioAnalyzer.Shared.Models;

namespace PortfolioAnalyzer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly IPortfolioService _portfolioService;
    private readonly IAnalyticsService _analyticsService;
    private readonly ILogger<AnalyticsController> _logger;

    public AnalyticsController(
        IPortfolioService portfolioService,
        IAnalyticsService analyticsService,
        ILogger<AnalyticsController> logger)
    {
        _portfolioService = portfolioService;
        _analyticsService = analyticsService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<PortfolioAnalytics>> GetAnalytics()
    {
        try
        {
            var portfolio = await _portfolioService.GetPortfolioAsync();
            var analytics = await _analyticsService.CalculateAnalyticsAsync(portfolio);
            return Ok(analytics);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating analytics");
            return StatusCode(500, "An error occurred while calculating analytics");
        }
    }
}
