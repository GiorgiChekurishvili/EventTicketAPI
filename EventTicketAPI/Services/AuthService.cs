using AutoMapper;
using EventTicketAPI.Dtos;
using EventTicketAPI.Entities;
using EventTicketAPI.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Net;
using System.Net.Mail;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace EventTicketAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IAuthRepository _repository;
        private readonly IConfiguration _config;
        public AuthService(IConfiguration config, IMapper mapper, IAuthRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
            _config = config;

        }

        public async Task<string> Login(string Email, string Password)
        {
            var user = await _repository.LoginRepository(Email, Password);
            if (user == null)
            {
                return null;
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Surname, user.LastName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokendescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = credentials
            };
            var tokenhandler = new JwtSecurityTokenHandler();
            var token = tokenhandler.CreateToken(tokendescriptor);

            return tokenhandler.WriteToken(token);
        }

        public async Task<UserRegisterDto> Register(UserRegisterDto user)
        {
            var usermap = _mapper.Map<User>(user);
            var repo = await _repository.RegisterRepository(usermap, user.Password);

            return user;
        }

        public Task SendEmail(string email)
        {
            string mail = "test.organization007@gmail.com";
            string password = "Test1234!";

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail,password)
            };
            Random random = new Random();

            string numbers = null;
            for (int i = 0; i < 5; i++)
            {
                numbers = random.Next(0, 10).ToString();
            }
            
            return client.SendMailAsync(new MailMessage
                (
                from: mail,
                to: email,
                subject: "Verification",
                numbers

                ));
        }

        public async Task<bool> UserExists(string email)
        {
            return await _repository.UserExistsRepository(email);
        }

        public async Task<UserReturnDto> VerifyUser(string token)
        {
            var user = await _repository.UserVerificationRepository(token);
            if (user == null)
            {
                return null;
            }
            return _mapper.Map<UserReturnDto>(user);
        }
        public async Task<UserReturnDto> ChangePassword(ResetPasswordDto resetPassword)
        {
            var change = await _repository.ChangePasswordRepository(resetPassword.Token, resetPassword.Password);
            if (change == null)
            {
                return null;
            }
            return _mapper.Map<UserReturnDto>(change);
        }

        public async Task<UserReturnDto> ForgetPassword(string email)
        {
            var forget = await _repository.ForgetPasswordRepository(email);
            if (forget == null)
            {
                return null;
            }
            return _mapper.Map<UserReturnDto>(forget);
        }
    }
}
