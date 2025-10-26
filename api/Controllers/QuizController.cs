using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Application.DTOs;
using api.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class QuizController(IQuizService quizService) : ControllerBase
    {
        private readonly IQuizService _quizService = quizService;

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateQuizDto quizDto)
        {
            var result = await _quizService.Create(quizDto);
            return result.Status ? Ok(result) : BadRequest(result);
        }

        [HttpGet("get/questions/{quizId}")]
        public async Task<IActionResult> GetQuizQuestions(Guid quizId)
        {
            var result = await _quizService.GetQuizQuestions(quizId);
            return result.Status ? Ok(result) : NotFound(result);
        }

        [HttpGet("get/quiz-summary/{quizId}")]
        public async Task<IActionResult> GetQuizSummary(Guid quizId)
        {
            var result = await _quizService.GetQuizSummary(quizId);
            return result.Status ? Ok(result) : NotFound(result);
        }

        [HttpGet("get/quiz-questions/{summaryId}")]
        public async Task<IActionResult> GetAllQuizQuestion(Guid summaryId)
        {
            var result = await _quizService.GetAllQuizQuestion(summaryId);
            return result.Status ? Ok(result) : NotFound(result);
        }

        [HttpPost("question/answer/{questionId}")]
        public async Task<IActionResult> AnswerQuestion(Guid questionId, [FromBody] SubmitAnswerDto dto)
        {
            var result = await _quizService.AnswerQuestion(questionId, dto);
            return result.Status ? Ok(result) : BadRequest(result);
        }
    }
}