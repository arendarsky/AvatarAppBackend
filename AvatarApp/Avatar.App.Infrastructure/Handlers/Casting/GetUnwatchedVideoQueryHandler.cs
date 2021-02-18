using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Avatar.App.Casting.Commands;
using Avatar.App.Casting.Models;
using Avatar.App.Infrastructure.Handlers.Abstract;
using MediatR;

namespace Avatar.App.Infrastructure.Handlers.Casting
{
    internal class GetUnwatchedVideoQueryHandler: AutoMapperEFHandler, IRequestHandler<GetUnwatchedVideoQuery, IQueryable<ContestantPerformance>>
    {
        public GetUnwatchedVideoQueryHandler(AvatarAppContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public Task<IQueryable<ContestantPerformance>> Handle(GetUnwatchedVideoQuery request, CancellationToken cancellationToken)
        {
            var watchedVideos = DbContext.WatchedVideos.Where(video => video.User.Guid == request.UserGuid).Select(video => video.Video);
            var unwatchedVideoOwners = DbContext.Videos
                .Where(video => video.IsApproved.HasValue && video.IsApproved.Value && video.IsActive)
                .Except(watchedVideos).Select(video => video.User);
            return Task.FromResult(Mapper.ProjectTo<ContestantPerformance>(unwatchedVideoOwners));
        }
    }
}
