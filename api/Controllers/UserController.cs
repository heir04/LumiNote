using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Application.DTOs;
using api.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUserService userService, IJwtService jwtService) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly IJwtService _jwtService = jwtService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDto dto)
        {
            var result = await _userService.Register(dto);
            return result.Status ? Ok(result) : BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _userService.Login(dto);
             if (result.Status == false || result.Data == null)
            {
                return BadRequest(result);
            }

            if (result.Data != null)
            {
                var token = _jwtService.GenerateToken(result.Data.Id, result.Data.Email);
                return Ok(new
                {
                    Token = token
                });
            }
            return Unauthorized(result.Message);
        }
    }
}
