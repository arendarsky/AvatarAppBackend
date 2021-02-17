using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Api.Models.Profile;
using Avatar.App.Authentication;
using Avatar.App.Communications;
using Avatar.App.SharedKernel.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Avatar.App.Profile;
using Avatar.App.Profile.Models;

namespace Avatar.App.Api.Controllers
{
    [Authorize]
    [Route("api/profile")]
    [ApiController]
    public class ProfileController : BaseAuthorizeController
    {
        private readonly IProfileComponent _profileComponent;
        private readonly ICommunicationsComponent _communicationsComponent;
        private readonly AvatarAppSettings _avatarAppSettings;

        public ProfileController(IProfileComponent profileComponent, IAuthenticationComponent authenticationComponent,
            ICommunicationsComponent communicationsComponent, IOptions<AvatarAppSettings> avatarAppOptions) : base(
            authenticationComponent)
        {
            _profileComponent = profileComponent;
            _communicationsComponent = communicationsComponent;
            _avatarAppSettings = avatarAppOptions.Value;
        }

        [Route("get")]
        [HttpGet]
        public async Task<PrivateContestantProfileView> GetPrivate()
        {
            var userGuid = GetUserGuid();
            var profile = await _profileComponent.GetPrivateAsync(userGuid);
            return new PrivateContestantProfileView(profile);
        }

        [Route("public/get")]
        [HttpGet]
        public async Task<PublicContestantProfileView> GetPublic(long id)
        {
            var profile = await _profileComponent.GetPublicAsync(id);
            return new PublicContestantProfileView(profile);
        }

        [Route("notifications")]
        [HttpGet]
        public async Task<IEnumerable<LikeNotificationView>> GetNotifications(int number, int skip)
        {
            var userGuid = GetUserGuid();
            var notifications = await _communicationsComponent.GetNotificationsAsync(userGuid, number, skip);
            return notifications.Select(notification => new LikeNotificationView(notification));
        }

        [Route("set_password")]
        [HttpPost]
        public async Task<bool> SetPassword(string oldPassword, string newPassword)
        {
            var user = await GetUser();
            var result = await AuthenticationComponent.TryChangePassword(user, oldPassword, newPassword);
            return result;
        }

        [Route("photo/upload")]
        [RequestSizeLimit(1048576)]
        [HttpPost]
        public async Task<string> UploadPhoto(IFormFile file)
        {
            if (!CheckFileExtension(file))
            {
                HttpContext.Response.StatusCode = 400;
                return null;
            }

            var userGuid = GetUserGuid();
            return await _profileComponent.UploadPhotoAsync(userGuid, file);
        }

        private bool CheckFileExtension(IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName);
            return _avatarAppSettings.AcceptedImageExtensions.Contains(fileExtension);
        }

        [Route("photo/get/{name}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<FileStreamResult> GetPhotoByName(string name)
        {
            try
            {
                var fileStream = await _profileComponent.GetPhotoStreamAsync(name);

                if (fileStream != null)
                {
                    return File(fileStream, "image/*", true);
                }
            }
            catch (IOException)
            {

            }

            HttpContext.Response.StatusCode = 404;
            return null;
        }

        [Route("update_profile")]
        [HttpPost]
        public async Task UpdateProfile(ProfileUpdate privateProfile)
        {
            var userGuid = GetUserGuid();
            await _profileComponent.UpdateProfileAsync(userGuid, privateProfile);
        }
    }
}