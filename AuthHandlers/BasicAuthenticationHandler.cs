using System;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using BuildRestApiNetCore.Services.Customers;
using BuildRestApiNetCore.Exceptions;

namespace BuildRestApiNetCore.AuthHandlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {

        private readonly ICustomerService _customerService;

        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, 
                    ILoggerFactory logger, 
                    UrlEncoder encoder, 
                    ISystemClock clock,
                    ICustomerService customerService) 
                    : base(options, logger, encoder, clock)
        {
            _customerService = customerService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if(!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Authorization header was not found");

            
            AuthenticationHeaderValue headerValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            if(headerValue.Parameter == null)
                return AuthenticateResult.Fail("Authentication failed. Please check entered values");
            var bytes = Convert.FromBase64String(headerValue.Parameter);
            var credentials = Encoding.UTF8.GetString(bytes).Split("|");
            string email = credentials[0];
            string password = credentials[1]; 

            try
            {
                var customer = await _customerService.GetCustomer(email, password);
                var claim = new[] { new Claim(ClaimTypes.Name, customer.Email) };
                var identity = new ClaimsIdentity(claim, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch(CustomerNotFoundException)
            {
                return AuthenticateResult.Fail("Invalid email or password");
            }
            catch
            {
                return AuthenticateResult.Fail("Authentication failed. Please check entered values");
            }
        }
    }
}