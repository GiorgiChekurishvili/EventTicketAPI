using EventTicketAPI.Dtos;

namespace EventTicketAPI.Services
{
    public interface IAuthService
    {
        Task<UserRegisterDto> Register(UserRegisterDto user);
        Task<string> Login(string Email, string Password);
        Task<bool> VerifyUser(string email);
        Task SendEmail(string email);
    }
}
