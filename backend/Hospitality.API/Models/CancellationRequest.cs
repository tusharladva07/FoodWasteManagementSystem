namespace Hospitality.API.Models;

public class CancellationRequest
{
    public string BookingName { get; set; } = string.Empty;

    public int CanceledCovers { get; set; }

    public decimal CancellationRate { get; set; }

    public decimal EstimatedWasteCost { get; set; }
}
