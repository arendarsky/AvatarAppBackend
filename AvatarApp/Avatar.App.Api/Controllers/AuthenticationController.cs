using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avatar.App.Api.Models;
using Avatar.App.Api.Models.Impl;
using Avatar.App.Entities.Models;
using Avatar.App.Service.Constants;
using Avatar.App.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Avatar.App.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IDistributedCache _distributedCache;

        public AuthenticationController(IAuthenticationService authenticationService, IDistributedCache distributedCache)
        {
            _authenticationService = authenticationService;
            _distributedCache = distributedCache;
        }

        [Route("send")]
        public async Task<OkResult> SendEmail(string email)
        {
            await _authenticationService.SendEmailAsync(email);
            return Ok();
        }

        [Route("confirm")]
        public async Task<JsonResult> ConfirmEmail(string email, string confirmCode)
        {
            var response = new ConfirmationResponseModel();
            if (!await _authenticationService.ConfirmEmailAsync(email, confirmCode)) return new JsonResult(response);
            response.SessionGuid = await _distributedCache.GetStringAsync(RP.Session + email);
            if (!string.IsNullOrWhiteSpace(response.SessionGuid)) return new JsonResult(response);
            response.SessionGuid = Guid.NewGuid().ToString();
            await _distributedCache.SetStringAsync(response.SessionGuid, email);
            return new JsonResult(response);
        }

        
    }
}
