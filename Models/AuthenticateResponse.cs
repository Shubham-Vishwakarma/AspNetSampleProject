using System;
using BuildRestApiNetCore.Models;

namespace BuildRestApiNetCore.Models
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }

        public AuthenticateResponse(Customer customer, string token)
        {
            Id = customer.Id;
            Name = customer.Name;
            Email = customer.Email;
            Token = token;
        }
    }
}