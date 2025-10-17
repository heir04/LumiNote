using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Application.DTOs;

namespace api.Application.Interface
{
    public interface ISummaryService
    {
        Task<BaseResponse<CreateSummaryDto>> Create(CreateSummaryDto dto);
        Task<BaseResponse<SummaryDto>> Get(Guid id);
        Task<BaseResponse<IEnumerable<SummaryDto>>> GetAllByCurrentUser();
    }
}