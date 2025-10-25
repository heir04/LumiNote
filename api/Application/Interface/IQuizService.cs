using api.Application.DTOs;

namespace api.Application.Interface
{
    public interface IQuizService
    {
        Task<BaseResponse<QuizDto>> Create(CreateQuizDto quizDto);
        // Task<BaseResponse<QuizDto>> Get(Guid id);
        Task<BaseResponse<IEnumerable<QuizQuestionDto>>> GetQuizQuestions(Guid quizId); 
        Task<BaseResponse<QuizSummaryDto>> GetQuizSummary(Guid quizId);
        Task<BaseResponse<IEnumerable<QuizQuestionDisplayDto>>> GetAllQuizQuestion(Guid quizId);
        Task<BaseResponse<AnswerResultDto>> AnswerQuestion(Guid questionId, SubmitAnswerDto dto);
    }
}