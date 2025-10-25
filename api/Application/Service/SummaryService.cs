using api.Application.DTOs;
using api.Application.Interface;
using api.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace api.Application.Service
{
    public class SummaryService(ApplicationContext context, ICurrentUserService currentUser, IGeminiService geminiService) : ISummaryService
    {
        private readonly ApplicationContext _context = context;
        private readonly ICurrentUserService _currentUser = currentUser;
        private readonly IGeminiService _geminiService = geminiService;
        public async Task<BaseResponse<SummaryDto>> Create(IFormFile file)
        {
            var response = new BaseResponse<SummaryDto>();
            if (file == null || file.Length == 0)
            {
                response.Message = "Please upload a valid PDF file.";
                return response;
            }
            try
            {
                var summaryEntity = await _geminiService.ProcessPdfAsync(file);
                response.Data = new SummaryDto
                {
                    Id = summaryEntity.Id,
                    Title = summaryEntity.Title,
                    UserId = summaryEntity.UserId,
                    Content = summaryEntity.Content,
                    CreatedAt = summaryEntity.CreatedAt
                };
            }
            catch (Exception ex)
            {
                response.Message = $"Error processing PDF: {ex.Message}";
                return response;
            }
            response.Status = true;
            response.Message = "Summary created successfully.";
            return response;
        }

        public async Task<BaseResponse<SummaryDto>> Get(Guid id)
        {
            var response = new BaseResponse<SummaryDto>();
            var summary = await _context.Summaries.FindAsync(id);
            if (summary == null || summary.UserId != _currentUser.GetUserId())
            {
                response.Message = "Summary not found.";
                return response;
            }

            var summaryDto = new SummaryDto
            {
                Id = summary.Id,
                Title = summary.Title,
                UserId = summary.UserId,
                Content = summary.Content,
                CreatedAt = summary.CreatedAt
            };

            response.Data = summaryDto;
            response.Status = true;
            response.Message = "Summary retrieved successfully.";
            return response;
        }

        public async Task<BaseResponse<IEnumerable<SummaryDto>>> GetAllByCurrentUser()
        {
            var response = new BaseResponse<IEnumerable<SummaryDto>>();
            var summaries = await _context.Summaries
                .Where(s => s.UserId == _currentUser.GetUserId())
                .ToListAsync();
            if (summaries is null)
            {
                response.Message = "No summaries found for the current user.";
                return response;
            }

            response.Data = summaries.Select(s => new SummaryDto
            {
                Id = s.Id,
                Title = s.Title,
                UserId = s.UserId,
                Content = s.Content,
                CreatedAt = s.CreatedAt
            });
            response.Status = true;
            response.Message = "Summaries retrieved successfully.";
            return response;
        }
    }
}