using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Avatar.App.Entities.Models;

namespace Avatar.App.Service.Services
{
    public interface IAuthenticationService
    {
        Task SendEmail(string email);
        Task<bool> ConfirmEmail(string email, string confirmCode);
    }
}
