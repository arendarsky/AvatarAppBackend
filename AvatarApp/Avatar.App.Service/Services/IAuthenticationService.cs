using System;
using System.Threading.Tasks;
using Avatar.App.Service.Models;

namespace Avatar.App.Service.Services
{
    public interface IAuthenticationService
    {
        Task<string> GetAuthorizationTokenAsync(string email, string password);

        Task<bool> RegisterAsync(UserDto userDto);
        Task SendEmailAsync(string email);
        Task<bool> ConfirmEmailAsync(string email, string confirmCode);

        Task<Guid> GetUserGuidAsync(string email);
    }
}
