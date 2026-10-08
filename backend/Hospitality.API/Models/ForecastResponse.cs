namespace Hospitality.API.Models;

public class ForecastResponse
{
    public int ExpectedCovers { get; set; }

    public decimal HistoricalCancellationRate { get; set; }

    public int RecommendedPrepCount { get; set; }

    public decimal TotalWasteKg { get; set; }

    public decimal TotalCumulativeDollarLoss { get; set; }
}
