﻿using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Avatar.App.Api.Models;
using Avatar.App.Entities.Models;
using Avatar.App.Service.Constants;
using Avatar.App.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Avatar.App.Api.Controllers
{
    [ApiController]
    [Route("api/video")]
    public class VideoController : ControllerBase
    {
        private readonly IVideoService _videoService;

        [Route("upload")]
        public async Task<IActionResult> Upload(IFormFile upploadedVideoFile)
        {
            await _videoService.Upload(upploadedVideoFile.OpenReadStream());
            return Ok();
        }
    }
}
