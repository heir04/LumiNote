using api.Application.DTOs;
using api.Application.Interface;
using api.Entities;
using api.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace api.Application.Service
{
    public class NoteService(ApplicationContext context, ICurrentUserService currentUser) : INoteService
    {
        private readonly ApplicationContext _context = context;
        private readonly ICurrentUserService _currentUser = currentUser;
        public async Task<BaseResponse<CreateNoteDto>> Create(CreateNoteDto dto)
        {
            var response = new BaseResponse<CreateNoteDto>();

            var note = new Note
            {
                UserId = _currentUser.GetUserId(),
                Content = dto.Content,
                SourceType = dto.SourceType,
                Title = dto.Title
            };
            await _context.Notes.AddAsync(note);
            await _context.SaveChangesAsync();
            response.Data = dto;
            response.Status = true;
            response.Message = "Note created successfully.";
            return response;
        }

        public async Task<BaseResponse<NoteDto>> Get(Guid id)
        {
            var response = new BaseResponse<NoteDto>();
            var note = await _context.Notes.FindAsync(id);
            if (note == null || note.UserId != _currentUser.GetUserId())
            {
                response.Status = false;
                response.Message = "Note not found.";
                return response;
            }

            var noteDto = new NoteDto
            {
                Id = note.Id,
                Title = note.Title,
                UserId = note.UserId,
                Content = note.Content,
                SourceType = note.SourceType,
                CreatedAt = note.CreatedAt
            };

            response.Data = noteDto;
            response.Status = true;
            return response;
        }

        public async Task<BaseResponse<IEnumerable<NoteDto>>> GetAllByCurrentUser()
        {
            var response = new BaseResponse<IEnumerable<NoteDto>>();
            var notes = await _context.Notes
                .Where(n => n.UserId == _currentUser.GetUserId())
                .ToListAsync();
            if (notes is null)
            {
                response.Message = "No notes found for the current user.";
                return response;
            }

            response.Data = notes.Select(n => new NoteDto
            {
                Id = n.Id,
                Title = n.Title,
                UserId = n.UserId,
                Content = n.Content,
                SourceType = n.SourceType,
                CreatedAt = n.CreatedAt
            });
            response.Status = true;
            response.Message = "Notes retrieved successfully.";
            return response;
        }

    }
}