using EventTicketAPI.Dtos;

namespace EventTicketAPI.Services
{
    public interface IAuthService
    {
        Task<UserRegisterDto> Register(UserRegisterDto user);
        Task<string> Login(string Email, string Password);
        Task<bool> UserExists(string email);
        Task SendEmail(string email);
        Task<UserReturnDto> VerifyUser(string token);
        Task<UserReturnDto> ChangePassword(ResetPasswordDto resetPassword);
        Task<UserReturnDto> ForgetPassword(string email);

    }
}
