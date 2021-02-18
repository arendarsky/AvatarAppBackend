using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Administration.Commands;
using Avatar.App.SharedKernel;
using Avatar.App.SharedKernel.Commands;
using Avatar.App.SharedKernel.Models;
using Avatar.App.SharedKernel.Settings;
using MediatR;
using Microsoft.Extensions.Options;
using Video = Avatar.App.Administration.Models.Video;

namespace Avatar.App.Administration.Schedulers
{
    internal class RemoveUnusedFilesScheduler: ICronInvocable
    {
        private readonly IMediator _mediator;
        private readonly IQueryManager _queryManager;
        private readonly AvatarAppSettings _settings;

        public RemoveUnusedFilesScheduler(IMediator mediator, IOptions<AvatarAppSettings> settingsOptions, IQueryManager queryManager)
        {
            _mediator = mediator;
            _queryManager = queryManager;
            _settings = settingsOptions.Value;
        }

        public async Task Invoke()
        {
            await RemoveVideoFiles();
            await RemoveImageFiles();
        }

        private async Task RemoveVideoFiles()
        {
            var videoQuery = await _mediator.Send(new GetQuery<Video>());
            var existedVideoFileNames = await _queryManager.ToListAsync(videoQuery.Select(video => video.Name));
            await _mediator.Send(new RemoveUnusedFiles(existedVideoFileNames, _settings.VideoStoragePrefix));
        }

        public async Task RemoveImageFiles()
        {
            var userQuery = await _mediator.Send(new GetQuery<BaseContestant>());
            var existedImageFileNames = await _queryManager.ToListAsync(userQuery.Select(user => user.ProfilePhoto));
            await _mediator.Send(new RemoveUnusedFiles(existedImageFileNames, _settings.ImageStoragePrefix));
        }
    }
}
