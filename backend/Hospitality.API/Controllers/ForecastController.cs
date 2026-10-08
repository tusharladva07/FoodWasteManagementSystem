namespace Hospitality.API.Controllers;

using Hospitality.API.Models;
using Hospitality.API.Repositories;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ForecastController : ControllerBase
{
    private readonly IHospitalityRepository _repository;

    public ForecastController(IHospitalityRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ForecastResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetForecast([FromQuery] int expectedCovers)
    {
        if (expectedCovers <= 0)
        {
            return BadRequest(new { message = "expectedCovers must be greater than 0." });
        }

        var cancellations = _repository.GetCancellations().ToList();

        decimal avgCancellationRate = 0m;
        if (cancellations.Count > 0)
        {
            avgCancellationRate = cancellations.Average(c => c.CancellationRate);
        }

        // Recommended Portions = Ceiling(Expected Covers * (1 - Average Cancellation Rate) * 1.10)
        decimal rawPortions = (decimal)expectedCovers * (1m - avgCancellationRate) * 1.10m;
        int recommendedPrepCount = (int)Math.Ceiling(rawPortions);

        var wasteRecords = _repository.GetWasteRecords().ToList();
        decimal totalWasteKg = wasteRecords.Sum(w => w.WastedWeightKg);
        decimal totalCumulativeDollarLoss = wasteRecords.Sum(w => w.EstimatedLossUsd);

        var response = new ForecastResponse
        {
            ExpectedCovers = expectedCovers,
            HistoricalCancellationRate = Math.Round(avgCancellationRate * 100m, 2),
            RecommendedPrepCount = recommendedPrepCount,
            TotalWasteKg = Math.Round(totalWasteKg, 2),
            TotalCumulativeDollarLoss = Math.Round(totalCumulativeDollarLoss, 2)
        };

        return Ok(response);
    }
}
