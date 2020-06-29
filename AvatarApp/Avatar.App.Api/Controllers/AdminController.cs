using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avatar.App.Api.Handlers;
using Avatar.App.Api.Models.UserModels;
using Avatar.App.SharedKernel;
using Avatar.App.Core.Exceptions;
using Avatar.App.Core.Models;
using Avatar.App.Core.Services;
using Avatar.App.SharedKernel.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;

namespace Avatar.App.Api.Controllers
{
    [Route("api/admin")]
    [Authorize]
    [ApiController]
    public class AdminController : BaseAuthorizeController
    {
        private readonly IVideoService _videoService;
        private readonly IProfileService _profileService;
        private readonly IEmailService _emailService;
        private readonly AvatarAppSettings _avatarAppSettings;

        public AdminController(IVideoService videoService, IProfileService profileService,
            IOptions<AvatarAppSettings> avatarAppOptions, IEmailService emailService)
        {
            _videoService = videoService;
            _profileService = profileService;
            _emailService = emailService;
            _avatarAppSettings = avatarAppOptions.Value;
        }

        /// <summary>
        /// Get list of unmoderated video names
        /// </summary>
        /// <param name="number">Number of requested video names</param>
        /// <response code="200">Returns video names</response>
        /// <response code="400">If some of the parameters are null</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">If something goes wrong on server</response>
        [SwaggerOperation("GetUncheckedVideoList")]
        [SwaggerResponse(statusCode: 200, type: typeof(ICollection<ModerationUserModel>), description: "List of video names")]
        [Route("get_videos")]
        [HttpGet]
        public async Task<ActionResult> GetUncheckedVideoList(int number)
        {
            try
            {
                CheckAdminRight();
                var uncheckedVideos = await _videoService.GetUncheckedVideosAsync(number);
                return new JsonResult(ConvertModelHandler.VideosToModerationUserModels(uncheckedVideos));
            }
            catch (UserNotAllowedException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                Logger.Log.LogError(ex.Message + ex.StackTrace);
                return Problem();
            }
        }

        /// <summary>
        /// Moderate video
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isApproved"></param>
        /// <response code="200">If video status successfully changes</response>
        /// <response code="400">If some of the parameters are null</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">If video doesn't exist on server</response>
        /// <response code="500">If something goes wrong on server</response>
        [SwaggerOperation("SetVideoStatus")]
        [Route("moderate")]
        [HttpGet]
        public async Task<ActionResult> SetVideoStatus(string name, bool isApproved)
        {
            try
            {
                CheckAdminRight();
                await _videoService.SetApproveStatusAsync(name, isApproved);
                return Ok();
            }
            catch (UserNotAllowedException)
            {
                return Forbid();
            }
            catch (VideoNotFoundException)
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
        /// Cleaning storage from unused video files
        /// </summary>
        /// <response code="200">If video status successfully changes</response>
        /// <response code="400">If some of the parameters are null</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">If video doesn't exist on server</response>
        /// <response code="500">If something goes wrong on server</response>
        [SwaggerOperation("CleanUpVideoStorage")]
        [Route("clean_video_files")]
        [HttpGet]
        public ActionResult CleanUpVideoStorage()
        {
            try
            {
                CheckAdminRight();
                _videoService.RemoveAllUnusedFiles();
                return Ok();
            }
            catch (UserNotAllowedException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                Logger.Log.LogError(ex.Message + ex.StackTrace);
                return Problem();
            }
        }

        /// <summary>
        /// Cleaning storage from unused video files
        /// </summary>
        /// <response code="200">If video status successfully changes</response>
        /// <response code="400">If some of the parameters are null</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">If video doesn't exist on server</response>
        /// <response code="500">If something goes wrong on server</response>
        [SwaggerOperation("CleanUpImageStorage")]
        [Route("clean_image_files")]
        [HttpGet]
        public ActionResult CleanUpImageStorage()
        {
            try
            {
                CheckAdminRight();
                _profileService.RemoveAllUnusedFiles();
                return Ok();
            }
            catch (UserNotAllowedException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                Logger.Log.LogError(ex.Message + ex.StackTrace);
                return Problem();
            }
        }

        /// <summary>
        /// Set Semifinalist
        /// </summary>
        /// <param name="userId"></param>
        /// <response code="200">If video status successfully changes</response>
        /// <response code="400">If some of the parameters are null</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">If video doesn't exist on server</response>
        /// <response code="500">If something goes wrong on server</response>
        [SwaggerOperation("SetSemifinalist")]
        [Route("set_semifinalist")]
        [HttpGet]
        public async Task SetSemifinalist(long userId)
        {
            try
            {
                CheckAdminRight();
                await _profileService.SetSemifinalistAsync(userId);
            }
            catch (UserNotAllowedException)
            {
            }
            catch (Exception ex)
            {
                Logger.Log.LogError(ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        /// Send general message
        /// </summary>
        /// <param name="generalMessage"></param>
        /// <response code="200">If video status successfully changes</response>
        /// <response code="400">If some of the parameters are null</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">If video doesn't exist on server</response>
        /// <response code="500">If something goes wrong on server</response>
        [SwaggerOperation("SendGeneralMessage")]
        [Route("send_general_message")]
        [HttpPost]
        public async Task SendGeneralMessage(GeneralMessageDto generalMessage)
        {
            try
            {
                CheckAdminRight();
                await _emailService.SendGeneralEmailMessage(generalMessage.Subject, generalMessage.Text);
            }
            catch (UserNotAllowedException)
            {
            }
            catch (Exception ex)
            {
                Logger.Log.LogError(ex.Message + ex.StackTrace);
            }
        }


        private void CheckAdminRight()
        {
            var userGuid = GetUserGuid();

            if (userGuid != Guid.Parse(_avatarAppSettings.AdminGuid)) throw new UserNotAllowedException();
        }

    }
}