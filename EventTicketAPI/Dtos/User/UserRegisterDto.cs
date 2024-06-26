﻿using System.ComponentModel.DataAnnotations;

namespace EventTicketAPI.Dtos.User
{
    public class UserRegisterDto
    {

        public required string Name { get; set; }
        public required string LastName { get; set; }
        [EmailAddress]
        public required string Email { get; set; }

        public required string Password { get; set; }
        [Compare("Password")]
        public required string ConfirmPassword { get; set; }
    }
}
