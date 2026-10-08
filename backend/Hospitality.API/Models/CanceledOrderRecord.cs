namespace Hospitality.API.Models;

public class CanceledOrderRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string BookingName { get; set; } = string.Empty;

    public int CanceledCovers { get; set; }

    public decimal CancellationRate { get; set; }

    public decimal EstimatedWasteCost { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
