using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avatar.App.Api.Handlers;
using Avatar.App.Api.Models.UserModels;
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
    public class ProfileController : BaseAuthorizeController
    {
        private readonly IProfileService _profileService;
        private readonly AvatarAppSettings _avatarAppSettings;

        public ProfileController(IProfileService profileService, IOptions<AvatarAppSettings> avatarAppOptions)
        {
            _profileService = profileService;
            _avatarAppSettings = avatarAppOptions.Value;
        }

        /// <summary>
        /// Get user private profile
        /// </summary>
        /// <response code="200">Returns user private profile</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">If something goes wrong on server</response>
        [SwaggerOperation("Get")]
        [SwaggerResponse(statusCode: 200, type: typeof(PrivateProfileUserModel), description: "User profile")]
        [Route("get")]
        [HttpGet]
        public async Task<ActionResult> Get()
        {

            try
            {
                var userGuid = GetUserGuid();
                var userProfile = await _profileService.GetAsync(userGuid);
                return new JsonResult(ConvertModelHandler.UserProfileToPrivateProfileUserModel(userProfile));
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

        /// <summary>
        /// Get user public profile
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Returns user public profile</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">User not found</response>
        /// <response code="500">If something goes wrong on server</response>
        [SwaggerOperation("GetPublic")]
        [SwaggerResponse(statusCode: 200, type: typeof(PublicProfileUserModel), description: "User profile")]
        [Route("public/get")]
        [HttpGet]
        public async Task<ActionResult> GetPublic(long id)
        {
            try
            {
                var user = await _profileService.GetPublicAsync(id);
                return new JsonResult(ConvertModelHandler.UserToPublicUserProfile(user));
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
        /// Get user's notifications
        /// </summary>
        /// <param name="number"></param>
        /// <param name="skip"></param>
        /// <response code="200">Returns user's notifications</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">If something goes wrong on server</response>
        [SwaggerOperation("GetNotifications")]
        [SwaggerResponse(statusCode: 200, type: typeof(ICollection<NotificationUserModel>), description: "User's notifications")]
        [Route("notifications")]
        [HttpGet]
        public async Task<ActionResult> GetNotifications(int number, int skip)
        {

            try
            {
                var userGuid = GetUserGuid();

                var userLikes = await _profileService.GetNotificationsAsync(userGuid, number, skip);

                return new JsonResult(ConvertModelHandler.LikedVideosToNotificationUserModel(userLikes));
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

        /// <summary>
        /// Set user description 
        /// </summary>
        /// <param name="description"></param>
        /// <response code="200"></response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">If something goes wrong on server</response>
        [SwaggerOperation("SetDescription")]
        [Route("set_description")]
        [HttpPost]
        public async Task<ActionResult> SetDescription(string description)
        {

            try
            {
                var userGuid = GetUserGuid();

                await _profileService.SetDescriptionAsync(userGuid, description);
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

        /// <summary>
        /// Set user name 
        /// </summary>
        /// <param name="name"></param>
        /// <response code="200"></response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">If something goes wrong on server</response>
        [SwaggerOperation("SetName")]
        [Route("set_name")]
        [HttpGet]
        public async Task<ActionResult> SetName(string name)
        {

            try
            {
                var userGuid = GetUserGuid();

                await _profileService.SetNameAsync(userGuid, name);
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

        /// <summary>
        /// Set user password 
        /// </summary>
        /// <remarks>
        ///     true - old password is correct and password changed
        ///     false - old password is incorrect
        /// </remarks>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <response code="200"></response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">If something goes wrong on server</response>
        [SwaggerOperation("SetPassword")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Is old password correct")]
        [Route("set_password")]
        [HttpPost]
        public async Task<ActionResult> SetPassword(string oldPassword, string newPassword)
        {

            try
            {
                var userGuid = GetUserGuid();

                await _profileService.SetPasswordAsync(userGuid, oldPassword, newPassword);
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

        /// <summary>
        /// Upload image to server
        /// </summary>
        /// <param name="file"></param>
        /// <response code="200">File successfully uploaded</response>
        /// <response code="400">If some of the parameters are null</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">If something goes wrong on server</response>
        [SwaggerOperation("UploadPhoto")]
        [SwaggerResponse(statusCode: 200, type: typeof(string), description: "New image file name")]
        [Route("photo/upload")]
        [RequestSizeLimit(1048576)]
        [HttpPost]
        public async Task<ActionResult> UploadPhoto(IFormFile file)
        {
            if (file == null) return BadRequest();
            var fileExtension = Path.GetExtension(file.FileName);

            try
            {
                var userGuid = GetUserGuid();

                return new JsonResult(await _profileService.UploadPhotoAsync(userGuid, file.OpenReadStream(), fileExtension));
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

        /// <summary>
        /// Get image stream by name
        /// </summary>
        /// <param name="name"></param>
        /// <response code="200">Returns image stream</response>
        /// <response code="400">If some of the parameters are null</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">If the image doesn't exist on server</response>
        /// <response code="500">If something goes wrong on server</response>
        [SwaggerOperation("GetPhotoByName")]
        [SwaggerResponse(statusCode: 200, type: typeof(FileStreamResult), description: "File stream")]
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
            catch (IOException)
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


        #endregion
    }
}