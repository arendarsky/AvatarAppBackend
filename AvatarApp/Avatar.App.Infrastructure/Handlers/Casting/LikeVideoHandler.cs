using System;
using System.Threading;
using System.Threading.Tasks;
using Avatar.App.Casting.Commands;
using Avatar.App.Infrastructure.Handlers.Abstract;
using Avatar.App.Infrastructure.Models.Casting;
using Avatar.App.SharedKernel.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Avatar.App.Infrastructure.Handlers.Casting
{
    internal class LikeVideoHandler: EFHandler, IRequestHandler<LikeVideo>
    {
        private readonly IMediator _mediator;

        public LikeVideoHandler(AvatarAppContext dbContext, IMediator mediator) : base(dbContext)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(LikeVideo request, CancellationToken cancellationToken)
        {
            var userId = await _mediator.Send(new GetUserIdByGuid(request.UserGuid), cancellationToken);
            var video = await GetAvailableVideo(request.VideoName, cancellationToken);

            if (video == null || await CheckViewExistence(userId, video.Id, cancellationToken))
            {
                return Unit.Value;
            }

            await DbContext.WatchedVideos.AddAsync(new WatchedVideoDb
            {
                UserId = userId,
                VideoId = video.Id,
                Date = DateTime.Now,
                IsLiked = request.IsLiked
            }, cancellationToken);
            return Unit.Value;
        }

        private async Task<VideoDb> GetAvailableVideo(string videoName, CancellationToken cancellationToken)
        {
            return await DbContext.Videos.FirstOrDefaultAsync(video =>
                string.Equals(video.Name, videoName) && video.IsApproved.HasValue && video.IsApproved.Value, cancellationToken);
        }

        private async Task<bool> CheckViewExistence(long userId, long videoId, CancellationToken cancellationToken)
        {
            return await DbContext.WatchedVideos.AnyAsync(video =>
                video.UserId == userId && video.VideoId == videoId, cancellationToken);
        }
    }
}
