using System;
using System.Linq;
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

        public VideoController(IVideoService videoService)
        {
            _videoService = videoService;
        }

        [Route("upload")]
        public async Task<IActionResult> Upload(IFormFile uploadedVideoFile)
        {
            if (uploadedVideoFile == null) return BadRequest();
            try
            {
                await _videoService.Upload(uploadedVideoFile.OpenReadStream());
                return Ok();
            }
            catch(Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
