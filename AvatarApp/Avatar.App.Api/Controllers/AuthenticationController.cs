﻿using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Avatar.App.Api.Models;
using Avatar.App.SharedKernel;
using Avatar.App.Core.Exceptions;
using Avatar.App.Core.Models;
using Avatar.App.Core.Security;
using Avatar.App.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace Avatar.App.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : BaseAuthorizeController
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
            try
            {
                if (!IsValidEmail(user.Email) || string.IsNullOrWhiteSpace(user.Password))
                    return BadRequest();
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
        public async Task SendEmail(string email)
        {
            await _authenticationService.SendEmailAsync(email);
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
        public async Task SendPasswordReset(string email)
        {
            await _authenticationService.SendPasswordReset(email);
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

        /// <summary>
        /// Set Firebase Id
        /// </summary>
        /// <param name="fireBaseId"></param>
        /// <response code="200"></response>
        /// <response code="400"></response>
        /// <response code="500"></response>
        [SwaggerOperation("SetFireBaseId")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Is confirmation successful")]
        [Authorize]
        [Route("firebase_set")]
        [HttpPost]
        public async Task<ActionResult> SetFireBaseId([FromBody] string fireBaseId)
        {
            try
            {
                var userGuid = GetUserGuid();

                await _authenticationService.SetFireBaseId(userGuid, fireBaseId);

                return Ok();
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

        #region Private Methods

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                    RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        #endregion

    }
}
