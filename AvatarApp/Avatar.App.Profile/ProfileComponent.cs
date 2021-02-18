using System;
using System.IO;
using System.Threading.Tasks;
using Avatar.App.Profile.Commands;
using Avatar.App.Profile.Models;
using Avatar.App.SharedKernel;
using Avatar.App.SharedKernel.Commands;
using Avatar.App.SharedKernel.Settings;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Avatar.App.Profile
{
    public interface IProfileComponent
    {
        Task<PrivateContestantProfile> GetPrivateAsync(Guid userGuid);
        Task<PublicContestantProfile> GetPublicAsync(long id);
        Task<string> UploadPhotoAsync(Guid userGuid, IFormFile file);
        Task UpdateProfileAsync(Guid userGuid, ProfileUpdate profileUpdate);
        Task SetVideoActiveAsync(Guid userGuid, string fileName);
        Task<FileStream> GetPhotoStreamAsync(string fileName);
    }

    internal class ProfileComponent: AvatarAppComponent, IProfileComponent
    {
        private readonly AvatarAppSettings _avatarAppSettings;

        public ProfileComponent(IMediator mediator, IOptions<AvatarAppSettings> avatarAppSettingsOptions, IQueryManager queryManager) : base(mediator, queryManager)
        {
            _avatarAppSettings = avatarAppSettingsOptions.Value;
        }

        public async Task<PrivateContestantProfile> GetPrivateAsync(Guid userGuid)
        {
            return await Mediator.Send(new GetUserByGuid<PrivateContestantProfile>(userGuid));
        }

        public async Task<PublicContestantProfile> GetPublicAsync(long id)
        {
            return await Mediator.Send(new GetById<PublicContestantProfile>(id));
        }

        public async Task<string> UploadPhotoAsync(Guid userGuid, IFormFile file)
        {
            var uploadCommand = new UploadContent(file, _avatarAppSettings.ImageStoragePrefix);
            var uploadTask = Mediator.Send(uploadCommand);
            var updateTask = Mediator.Send(new UpdateProfilePhoto(uploadCommand.FileName, userGuid));
            await Task.WhenAll(uploadTask, updateTask);
            return uploadCommand.FileName;
        }

        public async Task<FileStream> GetPhotoStreamAsync(string fileName)
        {
            return await Mediator.Send(new GetContent(_avatarAppSettings.ImageStoragePrefix, fileName));
        }

        public async Task UpdateProfileAsync(Guid userGuid, ProfileUpdate profileUpdate)
        {
            await Mediator.Send(new UpdateProfile(profileUpdate, userGuid));
        }

        public async Task SetVideoActiveAsync(Guid userGuid, string fileName)
        {
            await Mediator.Send(new SetVideoActive(fileName, userGuid));
        }
    }
}
