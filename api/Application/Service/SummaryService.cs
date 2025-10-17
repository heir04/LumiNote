using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Application.DTOs;
using api.Application.Interface;
using api.Entities;
using api.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace api.Application.Service
{
    public class SummaryService(ApplicationContext context, ICurrentUserService currentUser) : ISummaryService
    {
        private readonly ApplicationContext _context = context;
        private readonly ICurrentUserService _currentUser = currentUser;
        public async Task<BaseResponse<CreateSummaryDto>> Create(CreateSummaryDto dto)
        {
            var response = new BaseResponse<CreateSummaryDto>();

            var summary = new Summary
            {
                UserId = _currentUser.GetUserId(),
                Content = dto.Content,
                Title = dto.Title
            };

            await _context.Summaries.AddAsync(summary);
            await _context.SaveChangesAsync();
            response.Data = dto;
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