using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Casting.Commands;
using Avatar.App.Casting.CreationData;
using Avatar.App.Casting.Models;
using Avatar.App.Content.Commands;
using Avatar.App.SharedKernel;
using Avatar.App.SharedKernel.Commands;
using Avatar.App.SharedKernel.Settings;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Avatar.App.Casting
{
    public interface ICastingComponent
    {
        Task<string> UploadVideoAsync(IFormFile file, Guid userGuid);
        Task<bool> TrySetVideoFragmentIntervalAsync(Guid userGuid, string fileName, double startTime, double endTime);
        Task<FileStream> GetVideoStreamAsync(string fileName);
        Task RemoveVideoAsync(Guid userGuid, string fileName);
        Task<IEnumerable<ContestantPerformance>> GetUnwatchedVideosAsync(Guid userGuid, int number);
        Task SetLikeAsync(Guid userGuid, string fileName, bool isLike);
    }

    internal class CastingComponent: AvatarAppComponent, ICastingComponent
    {
        private readonly AvatarAppSettings _avatarAppSettings;

        public CastingComponent(IMediator mediator, IOptions<AvatarAppSettings> avatarAppSettingsOptions, IQueryManager queryManager) : base(mediator, queryManager)
        {
            _avatarAppSettings = avatarAppSettingsOptions.Value;
        }

        public async Task<string> UploadVideoAsync(IFormFile file, Guid userGuid)
        {
            var contestant = await GetContestantByGuid(userGuid);

            if (!CheckForAvailableVideoSlots(contestant))
            {
                return null;
            }

            var uploadCommand = new UploadContent(file, _avatarAppSettings.VideoStoragePrefix);
            var uploadTask = Mediator.Send(uploadCommand);
            var insertTask = Mediator.Send(new Insert<VideoCreation>(new VideoCreation(uploadCommand.FileName,
                contestant.Id, _avatarAppSettings.ShortVideoMaxLength, contestant.VideosNumber == 0)));
            await Task.WhenAll(uploadTask, insertTask);
            return uploadCommand.FileName;
        }

        private async Task<Contestant> GetContestantByGuid(Guid guid)
        {
            return await QueryManager.FirstOrDefaultAsync((await Mediator.Send(new GetQuery<Contestant>())).Where(contestant =>
                contestant.Guid == guid));
        }

        private bool CheckForAvailableVideoSlots(Contestant contestant)
        {
            return contestant.VideosNumber < _avatarAppSettings.MaxVideoNumber;
        }

        public async Task<bool> TrySetVideoFragmentIntervalAsync(Guid userGuid, string fileName, double startTime, double endTime)
        {
            var fragmentInterval =
                new VideoFragmentUpdate(fileName, startTime, endTime, _avatarAppSettings.ShortVideoMaxLength);
            if (!fragmentInterval.IsIntervalCorrect) return false;
            await Mediator.Send(new UpdateVideoFragmentInterval(fragmentInterval, userGuid));
            return true;
        }

        public async Task<FileStream> GetVideoStreamAsync(string fileName)
        {
            return await Mediator.Send(new GetContent(_avatarAppSettings.VideoStoragePrefix, fileName));
        }

        public async Task RemoveVideoAsync(Guid userGuid, string fileName)
        {
            await Mediator.Send(new RemoveVideo(userGuid, fileName));
        }

        public async Task<IEnumerable<ContestantPerformance>> GetUnwatchedVideosAsync(Guid userGuid, int number)
        {
            var unwatchedVideoQuery = await Mediator.Send(new GetUnwatchedVideoQuery(userGuid));
            return (await QueryManager.ToListAsync(unwatchedVideoQuery)).OrderBy(c => Guid.NewGuid()).Take(number); ;
        }

        public async Task SetLikeAsync(Guid userGuid, string fileName, bool isLike)
        {
            await Mediator.Send(new LikeVideo(userGuid, fileName, isLike));

            if (isLike)
            {
                //TODO: AddFireBaseNotification
            }
        }
    }
}
