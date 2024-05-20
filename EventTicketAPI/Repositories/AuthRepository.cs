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
            var token = CreateRandomToken();
            user.Verify = token;
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

        public async Task<User> UserVerification(string token)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x=>x.Verify == token);
            if (user == null)
            {
                return null;
            }
            user.VerifiedAt = DateTime.Now;
            user.Verify = null;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> ChangePassword(string token, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.PasswordChangeVerification == token);
            if (user == null || user.ExpiresPasswordChangeDate < DateTime.Now)
            {
                return null;
            }
            CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.ExpiresPasswordChangeDate = null;
            user.PasswordChangeVerification = null;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> ForgetPassword(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                return null;
            }
            var number = CreateRandomToken();
            user.PasswordChangeVerification = number;
            user.ExpiresPasswordChangeDate = DateTime.Now.AddDays(1);
            await _context.SaveChangesAsync();
            return user;
        }
        public string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(8));
        }
    }
    
}
