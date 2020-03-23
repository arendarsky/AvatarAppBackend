using System;
using System.Linq;
using System.Security.Claims;
using Avatar.App.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Avatar.App.Api.Controllers
{
    public abstract class BaseAuthorizeController : ControllerBase
    {
        protected Guid GetUserGuid()
        {
            var nameIdentifier = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (nameIdentifier == null) throw new UserNotFoundException();
            return Guid.Parse(nameIdentifier.Value);
        }
    }
}