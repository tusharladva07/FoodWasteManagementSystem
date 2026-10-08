namespace Hospitality.API.Models;

public class VisionAnalysisResult
{
    public string ItemName { get; set; } = string.Empty;

    public decimal TrayCapacityKg { get; set; }

    public decimal RemainingPercentage { get; set; }

    public decimal EstimatedLossUsd { get; set; }
}
