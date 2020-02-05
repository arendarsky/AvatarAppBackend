using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Avatar.App.Entities.Models;

namespace Avatar.App.Service.Services
{
    public interface IAuthenticationService
    {
        Task SendEmailAsync(string email);
        Task<bool> ConfirmEmailAsync(string email, string confirmCode);
    }
}
