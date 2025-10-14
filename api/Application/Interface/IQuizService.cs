using api.Application.DTOs;

namespace api.Application.Interface
{
    public interface IQuizService
    {
        Task<BaseResponse<QuizDto>> Create(CreateQuizDto quizDto);
        Task<BaseResponse<QuizDto>> Get(Guid id);
        Task<BaseResponse<IEnumerable<QuizSummaryDto>>> GetAllByCurrentUser();
        Task<BaseResponse<IEnumerable<QuizQuestionDisplayDto>>> GetAllQuizQuestion(Guid quizid);
        Task<BaseResponse<IEnumerable<QuizQuestionDto>>> GetAllQuestionsByQuizId(Guid quizId);
        Task<BaseResponse<AnswerResultDto>> AnswerQuestion(Guid questionId, SubmitAnswerDto dto);
    }
}