using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Avatar.App.Administration.Commands;
using Avatar.App.Administration.Models;
using Avatar.App.Infrastructure.Handlers.Abstract;
using MediatR;

namespace Avatar.App.Infrastructure.Handlers.Administration
{
    internal class GetUncheckedVideosHandler: AutoMapperEFHandler, IRequestHandler<GetUncheckedVideos, IQueryable<ModerationContestantPerformance>>
    {
        public GetUncheckedVideosHandler(AvatarAppContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public Task<IQueryable<ModerationContestantPerformance>> Handle(GetUncheckedVideos request, CancellationToken cancellationToken)
        {
            var videoQuery = DbContext.Videos.Where(video => !video.IsApproved.HasValue);
            return Task.FromResult(Mapper.ProjectTo<ModerationContestantPerformance>(videoQuery.Select(video => video.User)));
        }
    }
}
