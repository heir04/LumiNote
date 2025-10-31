using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Application.DTOs;

namespace api.Application.Interface
{
    public interface ISummaryService
    {
        Task<BaseResponse<SummaryDto>> Create(IFormFile file);
        Task<BaseResponse<SummaryDto>> Get(Guid id);
        Task<BaseResponse<IEnumerable<SummaryDto>>> GetAllByCurrentUser();
    }
}