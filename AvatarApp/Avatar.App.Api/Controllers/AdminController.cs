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

        [Route("get_videos")]
        [HttpGet]
        public ActionResult GetUncheckedVideoList(int number = 1)
        {
            try
            {
                var videoList = _videoService.GetUncheckedVideoList(number);
                return new JsonResult(videoList.Select(v => v.Name));
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        
    }
}