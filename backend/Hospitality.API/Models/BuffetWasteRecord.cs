namespace Hospitality.API.Models;

public class BuffetWasteRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string ItemName { get; set; } = string.Empty;

    public decimal TrayCapacityKg { get; set; }

    public decimal RemainingPercentage { get; set; }

    public decimal WastedWeightKg { get; set; }

    public decimal EstimatedLossUsd { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
