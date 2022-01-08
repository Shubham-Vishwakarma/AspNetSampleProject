using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BuildRestApiNetCore.Services.Auth;
using BuildRestApiNetCore.Models;
using BuildRestApiNetCore.Exceptions;

namespace BuildRestApiNetCore.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticateResponse>> Login(AuthenticateRequest request)
        {
            try
            {
                var user = await _service.AuthenticateUser(request);
                return Ok(user);
            }
            catch
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthenticateResponse>> Register(RegisterRequest request)
        {
            try
            {
                var user = await _service.RegisterUser(request);
                return Ok(user);
            }
            catch(CustomerDuplicateException)
            {
                return BadRequest(new { message = "User with email id alread exists" });
            }
            catch
            {
                return BadRequest(new { message = "Something went wrong" });
            }
        }
    }
}