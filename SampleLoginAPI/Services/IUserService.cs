using SampleLoginAPI.DTOs;

namespace SampleLoginAPI.Services
{
    public interface IUserService
    {
        Task<(bool IsSuccess, string Message)> RegisterUserAsync(UserRegisterDto dto);
        Task<(bool IsSuccess, string Message)> LoginUserAsync(UserLoginDto dto);

    }
}
