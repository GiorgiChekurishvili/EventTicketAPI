﻿using AutoMapper;
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
using EventTicketAPI.Dtos.User;

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
            if (user.VerifiedAt == null)
            {
                return null;
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Role, user.Role.Name)
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
