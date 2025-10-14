using api.Application.DTOs;
using api.Application.Interface;
using api.Infrastructure.Context;

namespace api.Application.Service
{
    public class NoteService(ApplicationContext context, ICurrentUserService currentUser) : INoteService
    {
        private readonly ApplicationContext _context = context;
        private readonly ICurrentUserService _currentUser = currentUser;
        public Task<BaseResponse<CreateNoteDto>> Create(CreateNoteDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<NoteDto>> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<NoteDto>> GetAllByCurrentUser()
        {
            throw new NotImplementedException();
        }
    }
}