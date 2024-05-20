using EventTicketAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Security.Cryptography;

namespace EventTicketAPI.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly EventTicketContext _context;
        public AuthRepository(EventTicketContext context)
        {

            _context = context;

        }
        public async Task<User> LoginRepository(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                return null;
            }
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }
            return user;

        }

        public async Task<User> RegisterRepository(User user, string password)
        {
            byte[] passwordhash, passwordsalt;
            CreatePasswordHash(password, out passwordhash, out passwordsalt);
            user.PasswordHash = passwordhash;
            user.PasswordSalt = passwordsalt;
            user.RoleId = 1;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UserExistsRepository(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email);
        }

        public bool VerifyPasswordHash(string password, byte[] PasswordHash, byte[] PasswordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA256(PasswordSalt))
            {
                var computehash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computehash.Length; i++)
                {
                    if (computehash[i] != PasswordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        public void CreatePasswordHash(string password, out byte[] PasswordHash, out byte[] PasswordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA256())
            {
                PasswordSalt = hmac.Key;
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
    
}
