using EventTicketAPI.Entities;

namespace EventTicketAPI.Repositories
{
    public interface IAuthRepository
    {
        Task<User> LoginRepository(string email, string password);
        Task<User> RegisterRepository(User user, string password);
        Task<bool> UserExistsRepository(string email);
        Task<User> UserVerificationRepository(string token);
        Task<User> ChangePasswordRepository(string token, string newPassword);
        Task<User> ForgetPasswordRepository(string email);
    }
}
