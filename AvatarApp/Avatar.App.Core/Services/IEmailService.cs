using System.Threading.Tasks;

namespace Avatar.App.Core.Services
{
    public interface IEmailService
    {
        Task SendConfirmCodeAsync(string email, string confirmCode, string guid);

        Task SendPasswordResetAsync(string email, string confirmCode, string guid);
    }
}
