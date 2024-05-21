using EventTicketAPI.Dtos;
using EventTicketAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.Security.Claims;
namespace EventTicketAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authservice;
        public AuthenticationController(IAuthService authService, IConfiguration configuration)
        {
            _authservice = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto userRegister)
        {
            if (userRegister == null)
            {
                return BadRequest();
            }
            userRegister.Email = userRegister.Email.ToLower();
            if (await _authservice.UserExists(userRegister.Email))
            {
                return BadRequest("User already exists");
            }
            UserRegisterDto user = new()
            {
                Email = userRegister.Email,
                Name = userRegister.Name,
                LastName = userRegister.LastName,
                Password = userRegister.Password,
                ConfirmPassword = userRegister.ConfirmPassword,
            };
            await _authservice.Register(user);
            return Ok();
        }
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserLoginDto userLogin)
        {
            if (userLogin == null)
            {
                return BadRequest();
            }
            var token = await _authservice.Login(userLogin.Email.ToLower(), userLogin.Password);
            if (token == null)
            {
                return BadRequest();
            }
            return Ok(token);
        }

        [HttpPost("verify/{token}")]
        public async Task<IActionResult> VerifyUser(string token)
        {
            var user = await _authservice.VerifyUser(token);
            if (user == null)
            {
                return BadRequest("Invalid Token");
            }
            return Ok("User Verified");
        }
        [HttpPost("forgetpassword/{email}")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            var forget = _authservice.ForgetPassword(email);
            if (forget == null)
            {
                return BadRequest("The email address could not be found");
            }
            return Ok("a key has been sent to your email");
        }
        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword(ResetPasswordDto resetPassword)
        {
            var change = _authservice.ChangePassword(resetPassword);
            if (change == null)
            {
                return BadRequest("Invalid Token");
            }
            return Ok("Password Changed Successfully");
        }
    }
}
