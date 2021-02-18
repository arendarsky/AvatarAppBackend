using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Avatar.App.Communications.Commands;
using Avatar.App.Communications.Models;
using Avatar.App.Infrastructure.Handlers.Abstract;
using MediatR;

namespace Avatar.App.Infrastructure.Handlers.Communications
{
    internal class GetLikeNotificationsQueryHandler: AutoMapperEFHandler, IRequestHandler<GetLikeNotificationsQuery, IQueryable<LikeNotification>>
    {
        public GetLikeNotificationsQueryHandler(AvatarAppContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public Task<IQueryable<LikeNotification>> Handle(GetLikeNotificationsQuery request, CancellationToken cancellationToken)
        {
            var likesQuery = DbContext.WatchedVideos.Where(w => w.Video.User.Guid == request.UserGuid && w.IsLiked);
            return Task.FromResult(Mapper.ProjectTo<LikeNotification>(likesQuery));
        }
    }
}
