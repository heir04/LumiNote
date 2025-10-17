using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Application.DTOs;
using api.Application.Interface;
using api.Infrastructure.Context;
using api.Infrastructure.Security;

namespace api.Application.Service
{
    public class QuizService(ApplicationContext context, ICurrentUserService currentUser) : IQuizService
    {
        private readonly ApplicationContext _context = context;
        private readonly ICurrentUserService _currentUser = currentUser;
        public Task<BaseResponse<AnswerResultDto>> AnswerQuestion(Guid questionId, SubmitAnswerDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<QuizDto>> Create(CreateQuizDto quizDto)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<QuizDto>> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<IEnumerable<QuizSummaryDto>>> GetAllByCurrentUser()
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<IEnumerable<QuizQuestionDto>>> GetAllQuestionsByQuizId(Guid quizId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<IEnumerable<QuizQuestionDisplayDto>>> GetAllQuizQuestion(Guid quizid)
        {
            throw new NotImplementedException();
        }
    }
}