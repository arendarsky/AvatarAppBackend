using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Avatar.App.Api.Models;
using Avatar.App.Api.Models.Impl;
using Avatar.App.Entities;
using Avatar.App.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;

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

        /// <summary>
        /// Send a confirmation code to the email.
        /// </summary>
        /// <param name="email"></param>
        /// <response code="200">Confirmation code was sent</response>
        /// <response code="400">If the parameter is null</response>
        /// <response code="500">If something goes wrong on server</response>
        [SwaggerOperation("SendEmail")]
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

        /// <summary>
        /// Check confirmation code and register user.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="confirmCode"></param>
        /// <response code="200">Confirmation codes are equal, the user successfully registered</response>
        /// <response code="400">If some of the parameters are null</response>
        /// <response code="500">If something goes wrong on server</response>
        [SwaggerOperation("ConfirmEmail")]
        [SwaggerResponse(statusCode: 200, type: typeof(ConfirmationResponseModel), description: "User token")]
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
