using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EventTicketAPI.Dtos
{
    public class UserLoginDto
    {
        [EmailAddress]
        public required string Email { get; set; }
        
        public required string Password { get; set; }
    }
}
