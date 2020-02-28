using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Avatar.App.Api.Models.Impl;
using Avatar.App.Entities;
using Avatar.App.Service.Models;
using Avatar.App.Service.Security;
using Avatar.App.Service.Services;
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
        /// Register new user
        /// </summary>
        /// <param name="user"></param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /register
        ///     {
        ///        "Name": "Ivan",
        ///        "Email": "example@ex.ru",
        ///        "Password": "asdaswqer123"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">User (not)successfully registered</response>
        /// <response code="400">If some of the required fields are null</response>
        /// <response code="500">If something goes wrong on server</response>
        [SwaggerOperation("Register")]
        [Route("register")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Is register successful")]
        [HttpPost]
        public async Task<ActionResult> Register(UserDto user)
        {
            if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
                return BadRequest();
            try
            {
                return new JsonResult(await _authenticationService.RegisterAsync(user));
            }
            catch(Exception ex)
            {
                Logger.Log.LogError(ex.Message + ex.StackTrace);
                return Problem();
            }
        }

        /// <summary>
        /// Get authorization token
        /// </summary>
        /// <remarks>
        ///     Token = null means password or email is incorrect
        /// </remarks>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <response code="200">Token sent</response>
        /// <response code="400">If some of the required parameters are null</response>
        /// <response code="500">If something goes wrong on server</response>
        [SwaggerOperation("GetAuthorizationToken")]
        [SwaggerResponse(statusCode: 200, type: typeof(AuthorizationResponseModel), description: "Authorization token")]
        [Route("authorize")]
        [HttpPost]
        public async Task<ActionResult> GetAuthorizationToken(string email, string password)
        {
            var response = new AuthorizationResponseModel();
            try
            {
                var token = await _authenticationService.GetAuthorizationTokenAsync(email, password);
                response.Token = token;
                return new JsonResult(response);
            }
            catch (Exception)
            {
                return Problem();
            }
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
        /// Check confirmation code and so validate email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="confirmCode"></param>
        /// <response code="200">Confirmation codes are equal/not equal</response>
        /// <response code="400">If some of the parameters are null</response>
        /// <response code="500">If something goes wrong on server</response>
        [SwaggerOperation("ConfirmEmail")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Is confirmation successful")]
        [Route("confirm")]
        [HttpGet]
        public async Task<ActionResult> ConfirmEmail(string email, string confirmCode)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(confirmCode)) return BadRequest();
            try
            {
                return new JsonResult(await _authenticationService.ConfirmEmailAsync(email, confirmCode));
            }
            catch(Exception ex)
            {
                Logger.Log.LogError(ex.Message + ex.StackTrace);
                return Problem();
            }
        }

        
    }
}
