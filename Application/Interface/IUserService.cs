using api.Application.DTOs;

namespace api.Application.Interface
{
    public interface IUserService
    {
        Task<BaseResponse<UserDto>> Login(LoginDto dto);
        Task<BaseResponse<UserDto>> Register(CreateUserDto dto);
    }
}