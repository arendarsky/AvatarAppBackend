using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Entities;
using Avatar.App.Service.Exceptions;
using Avatar.App.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace Avatar.App.Api.Controllers
{
    [Route("api/admin")]
    [Authorize]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IVideoService _videoService;

        public AdminController(IVideoService videoService)
        {
            _videoService = videoService;
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
        [SwaggerResponse(statusCode: 200, type: typeof(IEnumerable<string>), description: "List of video names")]
        [Route("get_videos")]
        [HttpGet]
        public async Task<ActionResult> GetUncheckedVideoList(int number = 1)
        {
            try
            {
                var videoList = await _videoService.GetUncheckedVideoListAsync(number);
                return new JsonResult(videoList.Select(v => v.Name));
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
                await _videoService.SetApproveStatusAsync(name, isApproved);
                return Ok();
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

        
    }
}