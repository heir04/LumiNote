using System.Text;
using api.Application.DTOs;
using api.Application.Interface;
using api.Entities;
using api.Infrastructure.Context;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using Newtonsoft.Json;

namespace api.Infrastructure.Service
{
    public class GeminiService : IGeminiService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;
        private readonly ApplicationContext _context;
        private readonly ICurrentUserService _currentUser;

        public GeminiService(IConfiguration config, IHttpClientFactory httpClientFactory, ApplicationContext context, ICurrentUserService currentUser)
        {
            _apiKey = config["Gemini:ApiKey"] ?? throw new Exception("Missing Gemini API key in configuration.");
            _httpClient = httpClientFactory.CreateClient();
            _context = context;
            _currentUser = currentUser;
            // Note: Gemini uses API key in query params, not Bearer token
        }

        public async Task<Summary> ProcessPdfAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Please upload a valid PDF file.");
                
            var userId = _currentUser.GetUserId();
            // Extract text from PDF (not stored to save space)
            string textContent = await ExtractPdfText(file);

            // Generate summary (this is what gets stored, not the original PDF content)
            var summaryText = await GenerateSummaryAsync(textContent);
            
            // Generate quiz questions
            var pdfQuestions = await GenerateQuestionsAsync(textContent);

            // Save summary entity
            var summaryEntity = new Summary
            {
                Title = Path.GetFileNameWithoutExtension(file.FileName),
                UserId = userId,
                Content = summaryText // Only storing the AI-generated summary
            };

            _context.Summaries.Add(summaryEntity);
            await _context.SaveChangesAsync();

            // Save quiz with questions (convert PdfQuestion to QuizQuestion)
            if (pdfQuestions.Any())
            {
                var quiz = new Quiz
                {
                    UserId = userId,
                    SummaryId = summaryEntity.Id
                };

                _context.Quizzes!.Add(quiz);
                await _context.SaveChangesAsync();

                // Convert PdfQuestion DTOs to QuizQuestion entities
                var quizQuestions = pdfQuestions.Select(pq => new QuizQuestion
                {
                    QuizId = quiz.Id,
                    QuestionText = pq.Question,
                    Options = pq.Options,
                    Answer = pq.Answer,
                    Explanation = pq.Explanation
                }).ToList();

                _context.QuizQuestions!.AddRange(quizQuestions);
                await _context.SaveChangesAsync();
            }

            return summaryEntity;
        }


        // ✅ Summarization
        public async Task<string> GenerateSummaryAsync(string documentText)
        {
            var model = "gemini-2.5-flash"; // Latest and fastest Gemini model
            var requestUrl = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={_apiKey}";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = $"Summarize this document in 3–5 paragraphs:\n{documentText}" }
                        }
                    }
                }
            };

            var response = await _httpClient.PostAsync(
                requestUrl,
                new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json")
            );

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(jsonResponse)!;

            return result?.candidates[0]?.content?.parts[0]?.text ?? "No summary generated.";
        }

        // ✅ Question Generation  
        public async Task<List<PdfQuestion>> GenerateQuestionsAsync(string documentText)
        {
            var model = "gemini-2.5-flash"; // Latest and fastest Gemini model
            var requestUrl = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={_apiKey}";

            var prompt = @$"
                Generate 5 multiple choice questions based on the following text.
                Return output strictly in JSON format:
                [
                {{ ""question"": """", ""options"": ["""", """", """", """"], ""answer"": """", ""explanation"": """" }}
                ]

                Text:
                {documentText}
                ";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            var response = await _httpClient.PostAsync(
                requestUrl,
                new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json")
            );

            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(jsonResponse)!;

            string textOutput = result?.candidates[0]?.content?.parts[0]?.text ?? "[]";

            // Clean potential markdown code blocks
            textOutput = textOutput.Trim();
            if (textOutput.StartsWith("```json"))
                textOutput = textOutput.Substring(7);
            if (textOutput.StartsWith("```"))
                textOutput = textOutput.Substring(3);
            if (textOutput.EndsWith("```"))
                textOutput = textOutput.Substring(0, textOutput.Length - 3);
            textOutput = textOutput.Trim();

            try
            {
                var parsedQuestions = JsonConvert.DeserializeObject<List<QuestionDto>>(textOutput);
                return parsedQuestions?.Select(q => new PdfQuestion
                {
                    Question = q.question ?? "No question text",
                    Options = q.options ?? new List<string>(),
                    Answer = q.answer ?? "N/A",
                    Explanation = q.explanation ?? ""
                }).ToList() ?? new List<PdfQuestion>();
            }
            catch
            {
                return new List<PdfQuestion>
                {
                    new PdfQuestion
                    {
                        Question = "Parsing failed — raw text stored instead.",
                        Options = new List<string> { textOutput },
                        Answer = "N/A",
                        Explanation = "Failed to parse response"
                    }
                };
            }
        }

        // Helper DTO for parsing Gemini JSON response
        private class QuestionDto
        {
            public string? question { get; set; }
            public List<string>? options { get; set; }
            public string? answer { get; set; }
            public string? explanation { get; set; }
        }

        public Task<string> ExtractPdfText(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            using var pdfReader = new PdfReader(stream);
            using var pdfDoc = new PdfDocument(pdfReader);

            string text = "";
            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
                text += PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i));

            return Task.FromResult(text);
        }
    }
}