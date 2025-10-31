using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class SummaryController(ISummaryService summaryService) : ControllerBase
    {
        private readonly ISummaryService _summaryService = summaryService;

        [HttpPost("create")]
        public async Task<IActionResult> Create(IFormFile file)
        {
            var result = await _summaryService.Create(file);
            return result.Status ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _summaryService.Get(id);
            return result.Status ? Ok(result) : NotFound(result);
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllByCurrentUser()
        {
            var result = await _summaryService.GetAllByCurrentUser();
            return result.Status ? Ok(result) : BadRequest(result);
        }
    }
}