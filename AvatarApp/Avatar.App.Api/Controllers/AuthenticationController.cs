﻿using System;
using System.Threading.Tasks;
using Avatar.App.Api.Models;
using Avatar.App.SharedKernel;
using Avatar.App.Core.Exceptions;
using Avatar.App.Core.Models;
using Avatar.App.Core.Security;
using Avatar.App.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace Avatar.App.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IJwtSigningEncodingKey _signingEncodingKey;

        public AuthenticationController(IAuthenticationService authenticationService, IJwtSigningEncodingKey signingEncodingKey)
        {
            _authenticationService = authenticationService;
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
                await _authenticationService.RegisterAsync(user);
                return new JsonResult(true);
            }
            catch (UserAlreadyExistsException)
            {
                return new JsonResult(false);
            }
            catch (Exception ex)
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
                var authModel = await _authenticationService.GetAuthorizationTokenAsync(email, password);
                response.Token = authModel.Token;
                response.ConfirmationRequired = authModel.ConfirmationRequired;
                return new JsonResult(response);
            }
            catch (UserNotFoundException)
            {
                return new JsonResult(response);
            }
            catch (InvalidPasswordException)
            {
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                Logger.Log.LogError(ex.Message + ex.StackTrace);
                return Problem();
            }
        }

        /// <summary>
        /// Send a email confirmation message to the email.
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
                return Ok();
            }
            catch(Exception ex)
            {
                Logger.Log.LogError(ex.Message + ex.StackTrace);
                return Problem();
            }
        }

        /// <summary>
        /// Check confirmation code and so validate email
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="confirmCode"></param>
        /// <response code="200">Confirmation codes are equal/not equal</response>
        /// <response code="400">If some of the parameters are null</response>
        /// <response code="500">If something goes wrong on server</response>
        [SwaggerOperation("ConfirmEmail")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Is confirmation successful")]
        [Route("confirm")]
        [HttpPost]
        public async Task<ActionResult> ConfirmEmail(string guid, string confirmCode)
        {
            if (string.IsNullOrWhiteSpace(guid) || string.IsNullOrWhiteSpace(confirmCode)) return BadRequest();
            try
            {
                return new JsonResult(await _authenticationService.ConfirmEmailAsync(guid, confirmCode));

            }
            catch (UserNotFoundException)
            {
                return NotFound();
            }
            catch(Exception ex)
            {
                Logger.Log.LogError(ex.Message + ex.StackTrace);
                return Problem();
            }
        }

        /// <summary>
        /// Send a password reset message to the email.
        /// </summary>
        /// <param name="email"></param>
        /// <response code="200">Password reset message was sent</response>
        /// <response code="400">If the parameter is null</response>
        /// <response code="500">If something goes wrong on server</response>
        [SwaggerOperation("SendPasswordReset")]
        [Route("send_reset")]
        [HttpGet]
        public async Task<ActionResult> SendPasswordReset(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return BadRequest("Parameter 'email' is null or empty");
            try
            {
                await _authenticationService.SendPasswordReset(email);
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.Log.LogError(ex.Message + ex.StackTrace);
                return Problem();
            }
        }

        /// <summary>
        /// Check confirmation code and change password
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="confirmCode"></param>
        /// <param name="password"></param>
        /// <response code="200">Confirmation codes are equal/not equal</response>
        /// <response code="400">If some of the parameters are null</response>
        /// <response code="500">If something goes wrong on server</response>
        [SwaggerOperation("ChangePassword")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Is confirmation successful")]
        [Route("password_change")]
        [HttpPost]
        public async Task<ActionResult> ChangePassword(string guid, string confirmCode, string password)
        {
            if (string.IsNullOrWhiteSpace(guid) || string.IsNullOrWhiteSpace(confirmCode)) return BadRequest();
            try
            {
                return new JsonResult(await _authenticationService.ChangePassword(guid, confirmCode, password));
            }
            catch (UserNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                Logger.Log.LogError(ex.Message + ex.StackTrace);
                return Problem();
            }
        }


    }
}
