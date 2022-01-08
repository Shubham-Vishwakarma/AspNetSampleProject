using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using BuildRestApiNetCore.Models;
using BuildRestApiNetCore.Services.Customers;
using BuildRestApiNetCore.Exceptions;

namespace BuildRestApiNetCore.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly ICustomerService _customerService;
        private readonly AppSettings _appSettings;

        public string ServiceName {
            get {
                return "AuthService";
            }
        }

        public AuthService(ICustomerService customerService, IOptions<AppSettings> appSettings)
        {
            _customerService = customerService;
            _appSettings = appSettings.Value;
        }

        public async Task<AuthenticateResponse> AuthenticateUser(AuthenticateRequest request)
        {
            var user = await _customerService.GetCustomer(request.Email, request.Password);
            var token = GenerateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public async Task<AuthenticateResponse> RegisterUser(RegisterRequest request)
        {
            bool found = await _customerService.CustomerExists(request.Email);

            Customer customer = new Customer();
            customer.Name = request.Name;
            customer.Email = request.Email;
            customer.Password = request.Password;

            customer = await _customerService.CreateCustomer(customer);

            var token = GenerateJwtToken(customer);

            return new AuthenticateResponse(customer, token);
        }

        private string GenerateJwtToken(Customer user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("Id", user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = System.DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}