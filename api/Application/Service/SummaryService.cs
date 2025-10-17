using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Application.DTOs;
using api.Application.Interface;
using api.Infrastructure.Context;

namespace api.Application.Service
{
    public class SummaryService(ApplicationContext context, ICurrentUserService currentUser) : ISummaryService
    {
        private readonly ApplicationContext _context = context;
        private readonly ICurrentUserService _currentUser = currentUser;
        public Task<BaseResponse<CreateSummaryDto>> Create(CreateSummaryDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<SummaryDto>> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SummaryDto>> GetAllByCurrentUser()
        {
            throw new NotImplementedException();
        }
    }
}