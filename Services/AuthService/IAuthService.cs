
using Backend.DTOs;
using Backend.Models;

namespace Backend.Services.AuthService
{
       public interface IAuthService{

            Task<string?> RegisterAsync(UserDto request);

            Task<string?> LoginAsync(UserDto request);
        }


}