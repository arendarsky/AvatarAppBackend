﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Security.Claims;
using Avatar.App.Entities;
using Avatar.App.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace Avatar.App.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/video")]
    public class VideoController : ControllerBase
    {
        private readonly IVideoService _videoService;

        public VideoController(IVideoService videoService)
        {
            _videoService = videoService;
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
        [Route("upload")]
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null) return BadRequest();
            var fileExtension = Path.GetExtension(file.FileName);
            var nameIdentifier = this.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (nameIdentifier == null) return Unauthorized();

            var userGuid = Guid.Parse(nameIdentifier.Value);
            try
            {
                await _videoService.UploadVideoAsync(file.OpenReadStream(), userGuid, fileExtension);
                return Ok();
            }
            catch (NullReferenceException)
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
        /// Get list of video names that are unwatched by current user
        /// </summary>
        /// <param name="number">Number of requested video names</param>
        /// <response code="200">Returns video names</response>
        /// <response code="400">If some of the parameters are null</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">If something goes wrong on server</response>
        [SwaggerOperation("GetUnwatchedVideos")]
        [SwaggerResponse(statusCode: 200, type: typeof(IEnumerable<string>), description: "List of video names")]
        [Route("get_unwatched")]
        [HttpGet]
        public async Task<ActionResult> GetUnwatchedVideos(int number)
        {
            var nameIdentifier = this.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (nameIdentifier == null) return Unauthorized();

            var userGuid = Guid.Parse(nameIdentifier.Value);
            try
            {
                var unwatchedVideos = await _videoService.GetUnwatchedVideoListAsync(userGuid, number);
                return new JsonResult(unwatchedVideos.Select(v => v.Name));
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
        /// <response code="500">If something goes wrong on server</response>
        [SwaggerOperation("GetVideoByName")]
        [SwaggerResponse(statusCode: 200, type: typeof(FileStreamResult), description: "File stream")]
        [Route("{name}")]
        [HttpGet]
        public async Task<ActionResult> GetVideoByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return BadRequest();
            try
            {
                var videoStream = await _videoService.GetVideoStreamAsync(name);
                if (videoStream == null) return NotFound();

                return File(videoStream, "video/*");
            }
            catch (Exception ex)
            {
                Logger.Log.LogError(ex.Message + ex.StackTrace);
                return Problem();
            }
        }
    }
}