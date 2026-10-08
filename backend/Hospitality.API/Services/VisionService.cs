namespace Hospitality.API.Services;

using Hospitality.API.Models;

public class VisionService : IVisionService
{
    private static readonly (string ItemName, decimal TrayCapacityKg, decimal RemainingPercentage, decimal EstimatedLossUsd)[] MockItems =
    [
        ("Scrambled Eggs", 10m, 30m, 45m),
        ("Crispy Bacon", 8m, 25m, 60m),
        ("Breakfast Sausages", 12m, 20m, 48m),
        ("Hash Browns", 6m, 35m, 24m),
        ("Baked Beans", 8m, 15m, 18m)
    ];

    public Task<VisionAnalysisResult> AnalyzeImageAsync(IFormFile imageFile)
    {
        // For demonstration purposes, select a realistic mock result.
        // If the uploaded file name matches a food item keyword, use that; otherwise default to Scrambled Eggs.
        var fileName = imageFile.FileName.ToLowerInvariant();
        var selected = MockItems.FirstOrDefault(m => fileName.Contains(m.ItemName.ToLowerInvariant()));

        if (string.IsNullOrEmpty(selected.ItemName))
        {
            selected = MockItems[0]; // Default: Scrambled Eggs
        }

        var result = new VisionAnalysisResult
        {
            ItemName = selected.ItemName,
            TrayCapacityKg = selected.TrayCapacityKg,
            RemainingPercentage = selected.RemainingPercentage,
            EstimatedLossUsd = selected.EstimatedLossUsd
        };

        return Task.FromResult(result);
    }
}
