using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avatar.App.Casting.Commands;
using Avatar.App.Infrastructure.Handlers.Abstract;
using Avatar.App.Infrastructure.Models.Casting;
using MediatR;

namespace Avatar.App.Infrastructure.Handlers.Casting
{
    internal class RemoveVideoHandler: EFHandler, IRequestHandler<RemoveVideo>
    {
        public RemoveVideoHandler(AvatarAppContext dbContext) : base(dbContext)
        {
        }

        public async Task<Unit> Handle(RemoveVideo request, CancellationToken cancellationToken)
        {
            var videoDb =
                await VideoDb.GetByNameAndUserGuidAsync(DbContext, request.UserGuid, request.FileName, cancellationToken);
            if (videoDb == null) return Unit.Value;
            var likedVideos = DbContext.LikedVideos.Where(likedVideo => likedVideo.VideoId == videoDb.Id);
            DbContext.LikedVideos.RemoveRange(likedVideos);
            var watchedVideos = DbContext.WatchedVideos.Where(likedVideo => likedVideo.VideoId == videoDb.Id);
            DbContext.WatchedVideos.RemoveRange(watchedVideos);
            DbContext.Videos.Remove(videoDb);
            await DbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
