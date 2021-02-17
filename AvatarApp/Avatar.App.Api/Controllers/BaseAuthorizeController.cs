using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Avatar.App.Authentication;
using Avatar.App.Authentication.Models;
using Microsoft.AspNetCore.Mvc;

namespace Avatar.App.Api.Controllers
{
    public abstract class BaseAuthorizeController : ControllerBase
    {
        protected IAuthenticationComponent AuthenticationComponent;

        protected BaseAuthorizeController(IAuthenticationComponent authenticationComponent)
        {
            AuthenticationComponent = authenticationComponent;
        }

        protected async Task<User> GetUser()
        {
            return await AuthenticationComponent.GetUser(GetUserGuid());
        }

        protected async Task<User> GetUser(string email)
        {
            return await AuthenticationComponent.GetUser(email);
        }

        protected async Task<User> GetUserByGuid(string guid)
        {
            if (!Guid.TryParse(guid, out var userGuid))
            {
                return null;
            }
            return await AuthenticationComponent.GetUser(userGuid);
        }

        protected Guid GetUserGuid()
        {
            var nameIdentifier = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return Guid.Parse(nameIdentifier.Value);
        }
    }
}