namespace Hospitality.API.Controllers;

using Hospitality.API.Models;
using Hospitality.API.Repositories;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IHospitalityRepository _repository;

    public OrdersController(IHospitalityRepository repository)
    {
        _repository = repository;
    }

    [HttpPost("cancellations")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult IngestCancellations([FromBody] List<CancellationRequest>? requests)
    {
        if (requests == null || requests.Count == 0)
        {
            return BadRequest(new { message = "At least one cancellation record is required." });
        }

        for (int i = 0; i < requests.Count; i++)
        {
            var req = requests[i];

            if (string.IsNullOrWhiteSpace(req.BookingName))
            {
                return BadRequest(new { message = $"Record at index {i}: BookingName is required." });
            }

            if (req.CanceledCovers < 0)
            {
                return BadRequest(new { message = $"Record at index {i}: CanceledCovers must be greater than or equal to 0." });
            }

            if (req.CancellationRate < 0m || req.CancellationRate > 1m)
            {
                return BadRequest(new { message = $"Record at index {i}: CancellationRate must be between 0 and 1." });
            }

            if (req.EstimatedWasteCost < 0m)
            {
                return BadRequest(new { message = $"Record at index {i}: EstimatedWasteCost must be greater than or equal to 0." });
            }
        }

        var savedRecords = new List<CanceledOrderRecord>();

        foreach (var req in requests)
        {
            var record = new CanceledOrderRecord
            {
                Id = Guid.NewGuid(),
                BookingName = req.BookingName.Trim(),
                CanceledCovers = req.CanceledCovers,
                CancellationRate = req.CancellationRate,
                EstimatedWasteCost = req.EstimatedWasteCost,
                Timestamp = DateTime.UtcNow
            };

            _repository.AddCancellation(record);
            savedRecords.Add(record);
        }

        return Ok(new
        {
            message = "Cancellations recorded successfully.",
            count = savedRecords.Count,
            records = savedRecords
        });
    }
}
