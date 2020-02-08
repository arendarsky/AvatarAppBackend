using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Avatar.App.Api.Models;
using Avatar.App.Api.Models.Impl;
using Avatar.App.Entities;
using Avatar.App.Entities.Models;
using Avatar.App.Service.Constants;
using Avatar.App.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Avatar.App.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IDistributedCache _distributedCache;
        private readonly IJwtSigningEncodingKey _signingEncodingKey;

        public AuthenticationController(IAuthenticationService authenticationService, IDistributedCache distributedCache, IJwtSigningEncodingKey signingEncodingKey)
        {
            _authenticationService = authenticationService;
            _distributedCache = distributedCache;
            _signingEncodingKey = signingEncodingKey;
        }

        [Route("send")]
        [HttpGet]
        public async Task<ActionResult> SendEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return BadRequest("Parameter 'email' is null or empty");
            try
            {
                await _authenticationService.SendEmailAsync(email);
            }
            catch(Exception ex)
            {
                Logger.Log.LogError(ex.Message + ex.StackTrace);
                return Problem();
            }
            return Ok();
        }

        [Route("confirm")]
        [HttpGet]
        public async Task<ActionResult> ConfirmEmail(string email, string confirmCode)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(confirmCode)) return BadRequest();
            var response = new ConfirmationResponseModel();
            try
            {
                if (!await _authenticationService.ConfirmEmailAsync(email, confirmCode))
                    return new JsonResult(response);
                var userGuid = await _authenticationService.GetUserGuidAsync(email);
                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userGuid.ToString())
                };
                var token = new JwtSecurityToken(
                    "AvatarApp",
                    "AvatarAppClient",
                    claims,
                    signingCredentials: new SigningCredentials(
                        _signingEncodingKey.GetKey(),
                        _signingEncodingKey.SigningAlgorithm));
                var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                response.Token = jwtToken;

                return new JsonResult(response);
            }
            catch(Exception ex)
            {
                Logger.Log.LogError(ex.Message + ex.StackTrace);
                return Problem();
            }
        }

        
    }
}
