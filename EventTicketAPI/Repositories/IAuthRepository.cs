using EventTicketAPI.Entities;

namespace EventTicketAPI.Repositories
{
    public interface IAuthRepository
    {
        Task<User> LoginRepository(string email, string password);
        Task<User> RegisterRepository(User user, string password);
        Task<bool> UserExistsRepository(string email);
    }
}
