using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using BuildRestApiNetCore.Models;
using BuildRestApiNetCore.Services.Customers;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace BuildRestApiNetCore.Middleware
{
    public class JwtMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        public JwtMiddleWare(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if(token != null)
                AttachUserToContext(context, token);

            await _next(context);                      
        }

        private void AttachUserToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters{
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validateToken);

                var jwtToken = (JwtSecurityToken)validateToken;
                int userId;
                int.TryParse(jwtToken.Claims.FirstOrDefault(x => x.Type == "Id")?.Value, out userId);
                var userEmail = jwtToken.Claims.FirstOrDefault(x => x.Type == "email")?.Value;
                var userName = jwtToken.Claims.FirstOrDefault(x => x.Type == "unique_name")?.Value;
                
                if(userId == 0 || string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(userName))
                    return;

                context.Items["User"] = new AuthenticateResponse(userId, userName, userEmail, token);
            }
            catch
            {
                throw;
            }
        }
    }
}