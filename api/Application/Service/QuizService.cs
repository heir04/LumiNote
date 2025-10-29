using api.Application.DTOs;
using api.Application.Interface;
using api.Entities;
using api.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace api.Application.Service
{
    public class QuizService(ApplicationContext context, ICurrentUserService currentUser) : IQuizService
    {
        private readonly ApplicationContext _context = context;
        private readonly ICurrentUserService _currentUser = currentUser;
        public async Task<BaseResponse<AnswerResultDto>> AnswerQuestion(Guid questionId, SubmitAnswerDto dto)
        {
            var response = new BaseResponse<AnswerResultDto>();

            var question = await _context.QuizQuestions.FindAsync(questionId);
            
            if (question == null)
            {
                response.Message = "Question not found.";
                return response;
            }

            // Authorization check: ensure the quiz belongs to the current user
            var quiz = await _context.Quizzes.FindAsync(question.QuizId);
            if (quiz == null || quiz.UserId != _currentUser.GetUserId())
            {
                response.Message = "Unauthorized to answer this question.";
                return response;
            }

            if (dto.UserAnswer.Equals(question.Answer, StringComparison.CurrentCultureIgnoreCase))
            {
                question.IsAnsweredCorrectly = true;
                question.IsAnswered = true;
                question.UserAnswer = dto.UserAnswer;
            }
            else
            {
                question.IsAnsweredCorrectly = false;
                question.IsAnswered = true;
                question.UserAnswer = dto.UserAnswer;
            }

            await _context.SaveChangesAsync();

            response.Status = true;
            response.Message = "Answer submitted successfully.";
            return response;
        }

        public async Task<BaseResponse<QuizDto>> Create(CreateQuizDto quizDto)
        {
            var response = new BaseResponse<QuizDto>();

            var quiz = new Quiz
            {
                CreatedAt = DateTime.UtcNow,
                UserId = _currentUser.GetUserId()
            };

            await _context.Quizzes.AddAsync(quiz);
            await _context.SaveChangesAsync();

            response.Status = true;
            response.Message = "Quiz created successfully.";
            return response;
        }

        // public async Task<BaseResponse<QuizDto>> Get(Guid id)
        // {
        //     var response = new BaseResponse<QuizDto>();

        //     var quiz = await _context.Quizzes.FindAsync(id);
        //     if (quiz == null)
        //     {
        //         response.Message = "Quiz not found.";
        //         return response;
        //     }

        //     response.Status = true;
        //     response.Data = new QuizDto
        //     {
        //         Id = quiz.Id,
        //         CreatedAt = quiz.CreatedAt
        //     };
        //     return response;
        // }

        public async Task<BaseResponse<IEnumerable<QuizQuestionDisplayDto>>> GetAllQuizQuestion(Guid summaryId)
        {
            var response = new BaseResponse<IEnumerable<QuizQuestionDisplayDto>>();
            
            var quizId = await _context.Quizzes
                .Where(q => q.SummaryId == summaryId && q.UserId == _currentUser.GetUserId())
                .Select(q => q.Id)
                .FirstOrDefaultAsync();
            if (quizId == Guid.Empty)
            {
                response.Message = "Quiz not found for the specified summary.";
                return response;
            }
            var questions = await _context.QuizQuestions
                .Where(q => q.QuizId == quizId)
                .ToListAsync();

            if (questions == null || !questions.Any())
            {
                response.Message = "No questions found for the specified quiz.";
                return response;
            }

            response.Status = true;
            response.Data = [.. questions.Select(q => new QuizQuestionDisplayDto
            {
                QuizId = q.QuizId,
                Id = q.Id,
                QuestionNumber = questions.IndexOf(q) + 1,
                QuestionText = q.QuestionText,
                Options = q.Options,
                IsAnswered = q.IsAnswered,
                UserAnswer = q.UserAnswer
            })];
            return response;
        }

        public async Task<BaseResponse<IEnumerable<QuizQuestionDto>>> GetQuizQuestions(Guid quizId)
        {
            var response = new BaseResponse<IEnumerable<QuizQuestionDto>>();

            var questions = await _context.QuizQuestions
                .Where(q => q.QuizId == quizId)
                .ToListAsync();
            
            if (questions == null || !questions.Any())
            {
                response.Message = "No questions found for the specified quiz.";
                return response;
            }

            response.Status = true;
            response.Data = [.. questions.Select(q => new QuizQuestionDto
            {
                QuizId = q.QuizId,
                QuestionText = q.QuestionText,
                Options = q.Options,
                Answer = q.Answer,
                IsAnswered = q.IsAnswered,
                IsAnsweredCorrectly = q.IsAnsweredCorrectly,
                Explanation = q.Explanation,
                UserAnswer = q.UserAnswer
            })];
            return response;
        }

        public async Task<BaseResponse<QuizSummaryDto>> GetQuizSummary(Guid quizId)
        {
            var response = new BaseResponse<QuizSummaryDto>();

            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(q => q.Id == quizId);

            if (quiz == null)
            {
                response.Message = "Quiz not found.";
                return response;
            }

            response.Status = true;
            response.Data = new QuizSummaryDto
            {
                Id = quiz.Id,
                TotalQuestions = quiz.Questions.Count,
                AnsweredCount = quiz.Questions.Count(q => q.IsAnswered),
                Score = quiz.Score,
                CreatedAt = quiz.CreatedAt
            };
            return response;
        }
    }
}