using System.Threading.Tasks;
using BuildRestApiNetCore.Models;

namespace BuildRestApiNetCore.Services.Auth
{
    public interface IAuthService : IService
    {
        Task<AuthenticateResponse> AuthenticateUser(AuthenticateRequest request);
        Task<AuthenticateResponse> RegisterUser(RegisterRequest request);
    }
}