using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Avatar.App.Authentication;
using Avatar.App.Authentication.Models;
using Avatar.App.Core.Exceptions;
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

        protected Guid GetUserGuid()
        {
            var nameIdentifier = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (nameIdentifier == null) throw new UserNotFoundException();
            return Guid.Parse(nameIdentifier.Value);
        }
    }
}