using EventTicketAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using MailKit.Net.Smtp;
using System.Net;
using System.Security.Cryptography;
using MimeKit;
using Org.BouncyCastle.Crypto.Macs;
using static System.Net.Mime.MediaTypeNames;
using MimeKit.Text;
using MailKit.Security;

namespace EventTicketAPI.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly EventTicketContext _context;
        private readonly IConfiguration _config;
        public AuthRepository(EventTicketContext context, IConfiguration config)
        {
            _config = config;
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
            SendEmail(user.Email, token);
            user.Verify = token;
            user.PasswordHash = passwordhash;
            user.PasswordSalt = passwordsalt;
            user.RoleId = 0;
            user.VerifiedAt = null;
            user.ExpiresPasswordChangeDate = null;
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

        public async Task<User> UserVerificationRepository(string token)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x=>x.Verify == token);
            if (user == null)
            {
                return null;
            }
            user.VerifiedAt = DateTime.Now;
            user.Verify = null;
            user.RoleId = 1;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> ChangePasswordRepository(string token, string newPassword)
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

        public async Task<User> ForgetPasswordRepository(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                return null;
            }
            var token = CreateRandomToken();
            SendEmail(email, token);
            user.PasswordChangeVerification = token;
            user.ExpiresPasswordChangeDate = DateTime.Now.AddDays(1);
            await _context.SaveChangesAsync();
            
            return user;
        }
        public string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(8));
        }
        public void SendEmail(string email, string token)
        {
            var mail = new MimeMessage();
            mail.From.Add(MailboxAddress.Parse(_config.GetSection("EmailSender").Value));
            mail.To.Add(MailboxAddress.Parse(email));
            mail.Subject = "Verification";
            mail.Body = new TextPart(TextFormat.Plain) { Text = token };

            using var smtp = new SmtpClient();
            smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_config.GetSection("EmailSender").Value, _config.GetSection("EmailPassword").Value);
            smtp.Send(mail);
            smtp.Disconnect(true);

        }
    }
    
}
