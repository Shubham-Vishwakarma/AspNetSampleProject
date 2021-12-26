using System;
using System.ComponentModel.DataAnnotations;

namespace BuildRestApiNetCore.Models
{
    public class AuthenticateRequest
    {
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}