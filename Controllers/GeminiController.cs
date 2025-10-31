using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GeminiController(IGeminiService geminiService) : ControllerBase
    {
        private readonly IGeminiService _geminiService = geminiService;

        [HttpPost("generate-summary")]
        public async Task<IActionResult> GenerateSummary([FromBody] string documentText)
        {
            if (string.IsNullOrWhiteSpace(documentText))
                return BadRequest("Document text cannot be empty.");

            var summary = await _geminiService.GenerateSummaryAsync(documentText);
            return Ok(summary);
        }

        [HttpPost("generate-questions")]
        public async Task<IActionResult> GenerateQuestions([FromBody] string documentText)
        {
            if (string.IsNullOrWhiteSpace(documentText))
                return BadRequest("Document text cannot be empty.");

            var questions = await _geminiService.GenerateQuestionsAsync(documentText);
            return Ok(questions);
        }

        [HttpPost("extract-pdf-text")]
        public async Task<IActionResult> ExtractPdfText(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Please upload a valid PDF file.");

            var textContent = await _geminiService.ExtractPdfText(file);
            return Ok(textContent);
        }
    }
}