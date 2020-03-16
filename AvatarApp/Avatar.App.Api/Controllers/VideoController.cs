using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Avatar.App.Api.Handlers;
using Avatar.App.Api.Models;
using Avatar.App.Api.Models.UserModels;
using Avatar.App.SharedKernel;
using Avatar.App.SharedKernel.Settings;
using Avatar.App.Core.Exceptions;
using Avatar.App.Core.Services;
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
    public class VideoController : BaseAuthorizeController
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
        [SwaggerResponse(statusCode: 200, type: typeof(string), description: "Video data")]
        [RequestSizeLimit(209715200)]
        [Route("upload")]
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {

            var fileExtension = Path.GetExtension(file.FileName);

            if (!CheckFileExtension(fileExtension)) return BadRequest();

            try
            {

                var userGuid = GetUserGuid();

                var video = await _videoService.UploadVideoAsync(file.OpenReadStream(), userGuid, fileExtension);

                return new JsonResult(video.Name);
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
        [SwaggerResponse(statusCode: 200, type: typeof(ICollection<VideoUserModel>), description: "List of video names")]
        [Route("get_unwatched")]
        [HttpGet]
        public async Task<ActionResult> GetUnwatchedVideos(int number)
        {

            try
            {
                var userGuid = GetUserGuid();

                var unwatchedVideos = await _videoService.GetUnwatchedVideosAsync(userGuid, number);
                return new JsonResult(ConvertModelHandler.VideosToVideoUserModels(unwatchedVideos));
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


            try
            {
                var userGuid = GetUserGuid();

                await _videoService.SetLikeAsync(userGuid, name, isLike);
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

            try
            {
                await _videoService.SetVideoFragmentInterval(userGuid, fileName, startTime, endTime);
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

            try
            {
                var userGuid = GetUserGuid();

                await _videoService.SetActiveAsync(userGuid, fileName);
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

            try
            {
                var userGuid = GetUserGuid();

                await _videoService.RemoveVideoAsync(userGuid, name);
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

        private bool CheckFileExtension(string fileExtension)
        {
            return fileExtension == _avatarAppSettings.AcceptedVideoExtension;
        }

        #endregion
    }
}