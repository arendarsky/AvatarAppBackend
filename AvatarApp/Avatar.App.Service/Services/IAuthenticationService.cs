using System;
using System.Threading.Tasks;

namespace Avatar.App.Service.Services
{
    public interface IAuthenticationService
    {
        Task SendEmailAsync(string email);
        Task<bool> ConfirmEmailAsync(string email, string confirmCode);

        Task<Guid> GetUserGuidAsync(string email);
    }
}
