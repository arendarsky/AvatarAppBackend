using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Security.Claims;
using Avatar.App.Api.Handlers;
using Avatar.App.Api.Models;
using Avatar.App.Entities;
using Avatar.App.Entities.Settings;
using Avatar.App.Service.Exceptions;
using Avatar.App.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;

namespace Avatar.App.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/video")]
    public class VideoController : ControllerBase
    {
        private readonly IVideoService _videoService;
        private readonly AvatarAppSettings _avatarAppSettings;

        public VideoController(IVideoService videoService, IOptions<AvatarAppSettings> avatarAppOptions)
        {
            _videoService = videoService;
            _avatarAppSettings = avatarAppOptions.Value;
        }

        /// <summary>
        /// Upload file on server
        /// </summary>
        /// <param name="file"></param>
        /// <response code="200">File successfully uploaded</response>
        /// <response code="400">If some of the parameters are null</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">If something goes wrong on server</response>
        [SwaggerOperation("Upload")]
        [SwaggerResponse(statusCode: 200, type: typeof(VideoModel), description: "Video data")]
        [RequestSizeLimit(209715200)]
        [Route("upload")]
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null) return BadRequest();
            var fileExtension = Path.GetExtension(file.FileName);
            var userGuid = GetUserGuid();
            if (!userGuid.HasValue) return Unauthorized();

            try
            {
                var video = await _videoService.UploadVideoAsync(file.OpenReadStream(), userGuid.Value, fileExtension);
                return new JsonResult(new VideoModel
                {
                    Name = video.Name
                });
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
            catch (ReachedVideoLimitException)
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
        /// Get list of video names that are unwatched by current user
        /// </summary>
        /// <param name="number">Number of requested video names</param>
        /// <response code="200">Returns video names</response>
        /// <response code="400">If some of the parameters are null</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">If something goes wrong on server</response>
        [SwaggerOperation("GetUnwatchedVideos")]
        [SwaggerResponse(statusCode: 200, type: typeof(ICollection<UserModel>), description: "List of video names")]
        [Route("get_unwatched")]
        [HttpGet]
        public async Task<ActionResult> GetUnwatchedVideos(int number)
        {
            var userGuid = GetUserGuid();
            if (!userGuid.HasValue) return Unauthorized();

            try
            {
                var unwatchedVideos = await _videoService.GetUnwatchedVideoListAsync(userGuid.Value, number);
                return new JsonResult(ConvertModelHandler.VideosToUserModels(unwatchedVideos));
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
        /// Get video stream by name
        /// </summary>
        /// <param name="name"></param>
        /// <response code="200">Returns video stream</response>
        /// <response code="400">If some of the parameters are null</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">If the video doesn't exist on server</response>
        /// <response code="500">If something goes wrong on server</response>
        [SwaggerOperation("GetVideoByName")]
        [SwaggerResponse(statusCode: 200, type: typeof(FileStreamResult), description: "File stream")]
        [AllowAnonymous]
        [Route("{name}")]
        [HttpGet]
        public async Task<ActionResult> GetVideoByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return BadRequest();
            try
            {
                var videoStream = await _videoService.GetVideoStreamAsync(name);
                if (videoStream == null) return NotFound();

                return File(videoStream, "video/*", true);
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

        /// <summary>
        /// Set video like
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isLike"></param>
        /// <response code="200">If video like successfully set</response>
        /// <response code="400">If some of the parameters are null</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">If something goes wrong on server</response>
        [SwaggerOperation("SetLike")]
        [Route("set_like")]
        [HttpGet]
        public async Task<ActionResult> SetLike(string name, bool isLike)
        {
            var userGuid = GetUserGuid();
            if (userGuid == null) return Unauthorized();

            try
            {
                await _videoService.SetLikeAsync(userGuid.Value, name, isLike);
                return Ok();
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
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

        [Route("set_interval")]
        [HttpGet]
        public async Task<ActionResult> SetVideoFragmentInterval(string fileName, double startTime, double endTime)
        {
            var userGuid = GetUserGuid();
            if (userGuid == null) return Unauthorized();

            try
            {
                await _videoService.SetVideoFragmentInterval(userGuid.Value, fileName, startTime, endTime);
                return Ok();
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
            catch (IncorrectFragmentIntervalException)
            {
                return BadRequest();
            }
            catch(VideoNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                Logger.Log.LogError(ex.Message + ex.StackTrace);
                return Problem();
            }
        }

        [Route("set_active")]
        [HttpGet]
        public async Task<ActionResult> SetActive(string fileName)
        {
            var userGuid = GetUserGuid();
            if (userGuid == null) return Unauthorized();

            try
            {
                await _videoService.SetActiveAsync(userGuid.Value, fileName);
                return Ok();
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
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

        [Route("remove/{name}")]
        [HttpGet]
        public async Task<ActionResult> Remove(string name)
        {
            var userGuid = GetUserGuid();
            if (userGuid == null) return Unauthorized();

            try
            {
                await _videoService.RemoveVideoAsync(userGuid.Value, name);
                return Ok();
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
            catch (VideoNotFoundException)
            {
                return NotFound();
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