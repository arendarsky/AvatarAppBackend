﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Avatar.App.Api.Handlers;
using Avatar.App.Api.Models;
using Avatar.App.SharedKernel;
using Avatar.App.SharedKernel.Settings;
using Avatar.App.Core.Exceptions;
using Avatar.App.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;

namespace Avatar.App.Api.Controllers
{
    [Authorize]
    [Route("api/profile")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        private readonly AvatarAppSettings _avatarAppSettings;

        public ProfileController(IProfileService profileService, IOptions<AvatarAppSettings> avatarAppOptions)
        {
            _profileService = profileService;
            _avatarAppSettings = avatarAppOptions.Value;
        }

        [Route("get")]
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var userGuid = GetUserGuid();
            if (!userGuid.HasValue) return Unauthorized();

            try
            {
                var userProfile = await _profileService.GetAsync(userGuid.Value);
                return new JsonResult(ConvertModelHandler.UserProfileToUserProfileModel(userProfile));
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                Logger.Log.LogError(ex.Message + ex.StackTrace);
                return Problem();
            }
        }

        [Route("notifications")]
        [SwaggerResponse(statusCode: 200, type: typeof(ICollection<LikedVideoModel>), description: "Notifications json")]
        [HttpGet]
        public async Task<ActionResult> GetNotifications(int number, int skip)
        {
            var userGuid = GetUserGuid();
            if (!userGuid.HasValue) return Unauthorized();
            try
            {
                var userLikes = await _profileService.GetNotificationsAsync(userGuid.Value, number, skip);

                return new JsonResult(ConvertModelHandler.LikedVideosToLikedVideoModels(userLikes));
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                Logger.Log.LogError(ex.Message + ex.StackTrace);
                return Problem();
            }
        }

        [Route("set_description")]
        [HttpPost]
        public async Task<ActionResult> SetDescription(string description)
        {
            var userGuid = GetUserGuid();
            if (!userGuid.HasValue) return Unauthorized();

            try
            {
                await _profileService.SetDescriptionAsync(userGuid.Value, description);
                return Ok();
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                Logger.Log.LogError(ex.Message + ex.StackTrace);
                return Problem();
            }
        }

        [Route("set_name")]
        [HttpGet]
        public async Task<ActionResult> SetName(string name)
        {
            var userGuid = GetUserGuid();
            if (!userGuid.HasValue) return Unauthorized();

            try
            {
                await _profileService.SetNameAsync(userGuid.Value, name);
                return Ok();
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                Logger.Log.LogError(ex.Message + ex.StackTrace);
                return Problem();
            }
        }

        [Route("set_password")]
        [HttpPost]
        public async Task<ActionResult> SetPassword(string oldPassword, string newPassword)
        {
            var userGuid = GetUserGuid();
            if (!userGuid.HasValue) return Unauthorized();

            try
            {
                await _profileService.SetPasswordAsync(userGuid.Value, oldPassword, newPassword);
                return new JsonResult(true);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
            catch (InvalidPasswordException)
            {
                return new JsonResult(false);
            }
            catch (Exception ex)
            {
                Logger.Log.LogError(ex.Message + ex.StackTrace);
                return Problem();
            }
        }

        [Route("photo/upload")]
        [RequestSizeLimit(1048576)]
        [HttpPost]
        public async Task<ActionResult> UploadPhoto(IFormFile file)
        {
            if (file == null) return BadRequest();
            var fileExtension = Path.GetExtension(file.FileName);
            var userGuid = GetUserGuid();
            if (!userGuid.HasValue) return Unauthorized();

            try
            {
                return new JsonResult(await _profileService.UploadPhotoAsync(userGuid.Value, file.OpenReadStream(), fileExtension));
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                Logger.Log.LogError(ex.Message + ex.StackTrace);
                return Problem();
            }
        }

        [Route("photo/get/{name}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> GetPhotoByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return BadRequest();
            try
            {
                var videoStream = await _profileService.GetPhotoStreamAsync(name);
                if (videoStream == null) return NotFound();

                return File(videoStream, "image/*", true);
            }
            catch (DirectoryNotFoundException)
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

        private Guid? GetUserGuid()
        {
            var nameIdentifier = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (nameIdentifier == null) return null;
            return Guid.Parse(nameIdentifier.Value);
        }

        #endregion
    }
}