using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using Avatar.App.Api.Models.Casting;
using Avatar.App.Authentication;
using Avatar.App.Casting;
using Avatar.App.Profile;
using Avatar.App.SharedKernel.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Avatar.App.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/video")]
    public class VideoController : BaseAuthorizeController
    {
        private readonly ICastingComponent _castingComponent;
        private readonly IProfileComponent _profileComponent;
        private readonly AvatarAppSettings _avatarAppSettings;

        public VideoController(ICastingComponent castingComponent, IProfileComponent profileComponent, IAuthenticationComponent authenticationComponent, IOptions<AvatarAppSettings> avatarAppOptions): base(authenticationComponent)
        {
            _castingComponent = castingComponent;
            _profileComponent = profileComponent;
            _avatarAppSettings = avatarAppOptions.Value;
        }

        [Route("upload")]
        [HttpPost]
        public async Task<string> Upload(IFormFile file)
        {
            if (!CheckFileExtension(file))
            {
                return null;
            }

            var userGuid = GetUserGuid();
            var videoName = await _castingComponent.UploadVideoAsync(file, userGuid);
            return videoName;
        }

        private bool CheckFileExtension(IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName);
            return fileExtension == _avatarAppSettings.AcceptedVideoExtension;
        }

        [Route("get_unwatched")]
        [HttpGet]
        public async Task<IEnumerable<CastingUser>> GetUnwatchedVideos(int number)
        {
            var userGuid = GetUserGuid();
            var unwatchedVideos = await _castingComponent.GetUnwatchedVideosAsync(userGuid, number);
            return unwatchedVideos.Select(video => new CastingUser(video));
        }

        [AllowAnonymous]
        [Route("{name}")]
        [HttpGet]
        public async Task<FileStreamResult> GetVideoByName(string name)
        {
            try
            {
                var videoStream = await _castingComponent.GetVideoStreamAsync(name);

                if (videoStream != null)
                {
                    return File(videoStream, "video/*", true);
                }
            }
            catch (IOException)
            {
               
            }

            HttpContext.Response.StatusCode = 404;
            return null;
        }

        [Route("set_like")]
        [HttpGet]
        public async Task SetLike(string name, bool isLike)
        {
            var userGuid = GetUserGuid();
            await _castingComponent.SetLikeAsync(userGuid, name, isLike);
        }

        [Route("set_interval")]
        [HttpGet]
        public async Task SetVideoFragmentInterval(string fileName, double startTime, double endTime)
        {
            var userGuid = GetUserGuid();
            var result =
                await _castingComponent.TrySetVideoFragmentIntervalAsync(userGuid, fileName, startTime, endTime);

            if (!result)
            {
                HttpContext.Response.StatusCode = 400;
            }
        }

        [Route("remove/{name}")]
        [HttpGet]
        public async Task Remove(string name)
        {
            var userGuid = GetUserGuid();
            await _castingComponent.RemoveVideoAsync(userGuid, name);
        }


        [Route("set_active")]
        [HttpGet]
        public async Task SetActive(string fileName)
        {
            var userGuid = GetUserGuid();
            await _profileComponent.SetVideoActiveAsync(userGuid, fileName);
        }
    }
}