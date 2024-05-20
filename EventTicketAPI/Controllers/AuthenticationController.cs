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
            if (await _authservice.VerifyUser(userRegister.Email))
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


    }
}
