using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avatar.App.Infrastructure.Handlers.Abstract;
using Avatar.App.Rating.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Avatar.App.Infrastructure.Handlers.Rating
{
    internal class GetLikesNumberHandler: EFHandler, IRequestHandler<GetLikesNumber, int>
    {
        public GetLikesNumberHandler(AvatarAppContext dbContext) : base(dbContext)
        {
        }

        public async Task<int> Handle(GetLikesNumber request, CancellationToken cancellationToken)
        {
            if (request.UserId.HasValue)
            {
                return await DbContext.WatchedVideos.Where(video => video.IsLiked && video.Video.UserId == request.UserId)
                    .CountAsync(cancellationToken);
            }

            if (request.UserGuid.HasValue)
            {
                return await DbContext.WatchedVideos.Where(video => video.IsLiked && video.Video.User.Guid == request.UserGuid)
                    .CountAsync(cancellationToken);
            }

            return 0;
        }
    }
}
