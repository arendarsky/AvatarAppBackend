using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Avatar.App.Service.Services
{
    public interface IEmailService
    {
        Task SendConfirmCodeAsync(string email, string confirmCode);
    }
}
