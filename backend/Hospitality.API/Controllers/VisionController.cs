namespace Hospitality.API.Controllers;

using Hospitality.API.Models;
using Hospitality.API.Repositories;
using Hospitality.API.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class VisionController : ControllerBase
{
    private readonly IVisionService _visionService;
    private readonly IHospitalityRepository _repository;

    public VisionController(IVisionService visionService, IHospitalityRepository repository)
    {
        _visionService = visionService;
        _repository = repository;
    }

    [HttpPost("analyze")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(BuffetWasteRecord), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Analyze(IFormFile? file)
    {
        var uploadedFile = file ?? (Request.HasFormContentType && Request.Form.Files.Count > 0 ? Request.Form.Files[0] : null);

        if (uploadedFile == null || uploadedFile.Length == 0)
        {
            return BadRequest(new { message = "An image file must be provided." });
        }

        var analysis = await _visionService.AnalyzeImageAsync(uploadedFile);

        var wastedWeightKg = Math.Round(analysis.TrayCapacityKg * analysis.RemainingPercentage / 100m, 2);

        var record = new BuffetWasteRecord
        {
            Id = Guid.NewGuid(),
            ItemName = analysis.ItemName,
            TrayCapacityKg = analysis.TrayCapacityKg,
            RemainingPercentage = analysis.RemainingPercentage,
            WastedWeightKg = wastedWeightKg,
            EstimatedLossUsd = analysis.EstimatedLossUsd,
            Timestamp = DateTime.UtcNow
        };

        _repository.AddWasteRecord(record);

        return Ok(record);
    }
}
