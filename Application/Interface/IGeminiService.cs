using api.Application.DTOs;
using api.Entities;

namespace api.Application.Interface
{
    public interface IGeminiService
    {
        Task<string> GenerateSummaryAsync(string documentText);
        Task<List<PdfQuestion>> GenerateQuestionsAsync(string documentText);
        Task<string> ExtractPdfText(IFormFile file);
        Task<Summary> ProcessPdfAsync(IFormFile file);
    }
}