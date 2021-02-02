using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avatar.App.Casting.Commands;
using Avatar.App.Infrastructure.CommandHandlers.Basic;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Avatar.App.Infrastructure.CommandHandlers.Casting
{
    internal class GetLikesNumberHandler: EFCommandHandler, IRequestHandler<GetLikesNumber, int>
    {
        public GetLikesNumberHandler(AvatarAppContext dbContext) : base(dbContext)
        {
        }

        public async Task<int> Handle(GetLikesNumber request, CancellationToken cancellationToken)
        {
            return await DbContext.LikedVideos.Where(video => video.Video.UserId == request.UserId)
                .CountAsync(cancellationToken);
        }
    }
}
