using api.Application.DTOs;

namespace api.Application.Interface
{
    public interface INoteService
    {
        Task<BaseResponse<CreateNoteDto>> Create(CreateNoteDto dto);
        Task<BaseResponse<NoteDto>> Get(Guid id);
        Task<BaseResponse<IEnumerable<NoteDto>>> GetAllByCurrentUser();
    }
}