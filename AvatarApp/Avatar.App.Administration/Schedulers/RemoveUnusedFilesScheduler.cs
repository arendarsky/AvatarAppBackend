using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Administration.Commands;
using Avatar.App.Casting.Models;
using Avatar.App.Schedulers;
using Avatar.App.SharedKernel.Commands;
using Avatar.App.SharedKernel.Settings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Video = Avatar.App.Administration.Models.Video;

namespace Avatar.App.Administration.Schedulers
{
    internal class RemoveUnusedFilesScheduler: ICronInvocable
    {
        private readonly IMediator _mediator;
        private readonly AvatarAppSettings _settings;

        public RemoveUnusedFilesScheduler(IMediator mediator, IOptions<AvatarAppSettings> settingsOptions)
        {
            _mediator = mediator;
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
            var existedVideoFileNames = await videoQuery.Select(video => video.Name).ToListAsync();
            await _mediator.Send(new RemoveUnusedFiles(existedVideoFileNames, _settings.VideoStoragePrefix));
        }

        public async Task RemoveImageFiles()
        {
            var userQuery = await _mediator.Send(new GetQuery<Contestant>());
            var existedImageFileNames = await userQuery.Select(user => user.ProfilePhoto).ToListAsync();
            await _mediator.Send(new RemoveUnusedFiles(existedImageFileNames, _settings.ImageStoragePrefix));
        }
    }
}
