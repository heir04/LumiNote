using api.Application.DTOs;
using api.Application.Interface;
using api.Entities;
using api.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace api.Application.Service
{
    public class UserService(ApplicationContext context) : IUserService
    {
        private readonly ApplicationContext _context = context;

        public async Task<BaseResponse<UserDto>> Login(LoginDto loginDto)
        {
            var response = new BaseResponse<UserDto>();
            var user = await _context.Users.FirstOrDefaultAsync(x =>
                x.Email == loginDto.Email && !x.IsDeleted);

            if (user is null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.HashedPassword))
            {
                response.Message = "Incorrect Email  or password!";
                return response;
            }

            response.Data = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
            };
            response.Message = "Welcome";
            response.Status = true;

            return response;
        }

        public async Task<BaseResponse<UserDto>> Register(CreateUserDto dto)
        {
            var response = new BaseResponse<UserDto>();

            var existingUser = await _context.Users.AnyAsync(x => x.Email == dto.Email && !x.IsDeleted);
            if (existingUser)
            {
                response.Message = "User already exists.";
                return response;
            }

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                HashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            response.Message = "User registered successfully.";
            response.Status = true;
            return response;
        }
    }
}