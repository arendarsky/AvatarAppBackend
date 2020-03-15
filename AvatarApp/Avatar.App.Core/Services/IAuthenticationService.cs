using System.Threading.Tasks;
using Avatar.App.Core.Models;

namespace Avatar.App.Core.Services
{
    public interface IAuthenticationService
    {
        Task<string> GetAuthorizationTokenAsync(string email, string password);

        Task RegisterAsync(UserDto userDto);
        Task SendEmailAsync(string email);
        Task<bool> ConfirmEmailAsync(string email, string confirmCode);
    }
}
