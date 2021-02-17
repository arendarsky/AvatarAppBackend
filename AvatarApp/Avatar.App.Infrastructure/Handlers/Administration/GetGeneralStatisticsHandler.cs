using System.Threading;
using System.Threading.Tasks;
using Avatar.App.Administration.Commands;
using Avatar.App.Administration.Models;
using Avatar.App.Infrastructure.Handlers.Abstract;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Avatar.App.Infrastructure.Handlers.Administration
{
    internal class GetGeneralStatisticsHandler: EFHandler, IRequestHandler<GetGeneralStatistics, GeneralStatistics>
    {
        public GetGeneralStatisticsHandler(AvatarAppContext dbContext) : base(dbContext)
        {
        }

        public async Task<GeneralStatistics> Handle(GetGeneralStatistics request, CancellationToken cancellationToken)
        {
            var statistics = new GeneralStatistics
            {
                TotalUsers = await DbContext.Users.CountAsync(cancellationToken),
                TotalActiveVideos = await DbContext.Videos.CountAsync(video => video.IsActive, cancellationToken),
                TotalVideos = await DbContext.Videos.CountAsync(cancellationToken),
                TotalLikes = await DbContext.WatchedVideos.CountAsync(video => video.IsLiked, cancellationToken),
                TotalViews = await DbContext.WatchedVideos.CountAsync(cancellationToken)
            };

            return statistics;
        }
    }
}
