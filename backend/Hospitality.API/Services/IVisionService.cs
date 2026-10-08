namespace Hospitality.API.Services;

using Hospitality.API.Models;

public interface IVisionService
{
    Task<VisionAnalysisResult> AnalyzeImageAsync(IFormFile imageFile);
}
