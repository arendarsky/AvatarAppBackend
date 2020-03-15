using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Avatar.App.Core.Exceptions;
using Microsoft.AspNetCore.Http;
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