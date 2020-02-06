using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

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

        [Route("get_video")]
        public async Task<ActionResult> GetVideo()
        {
            var videoStream = await _videoService.GetUncheckedVideoAsync();
            if (videoStream == null) return NoContent();

            return File(videoStream.Stream, "video/*", videoStream.Name);
        }

        
    }
}