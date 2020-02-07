using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Claims;
using Avatar.App.Api.Models;
using Avatar.App.Entities.Models;
using Avatar.App.Service.Constants;
using Avatar.App.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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

        [Route("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null) return BadRequest();
            var fileExtension = Path.GetExtension(file.FileName);
            var nameIdentifier = this.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (nameIdentifier == null) return Unauthorized();

            var userGuid = Guid.Parse(nameIdentifier.Value);
            try
            {
                await _videoService.UploadAsync(file.OpenReadStream(), userGuid, fileExtension);
                return Ok();
            }
            catch (NullReferenceException)
            {
                return Unauthorized();
            }
            catch (Exception)
            {
                return Problem();
            }
        }
        
        [Route("get_video")]
        public async Task<ActionResult> GetModeratedVideo()
        {
            try
            {
                var videoStream = await _videoService.GetModeratedVideoAsync();
                if (videoStream == null) return Ok();

                return File(videoStream.Stream, "video/*", videoStream.Name);
            }
            catch (Exception)
            {
                return Problem();
            }
        }
    }
}
