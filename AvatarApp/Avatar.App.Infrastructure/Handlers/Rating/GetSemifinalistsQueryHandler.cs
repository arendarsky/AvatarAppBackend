using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Avatar.App.Infrastructure.Handlers.Abstract;
using Avatar.App.Rating.Commands;
using Avatar.App.Rating.Models;
using MediatR;

namespace Avatar.App.Infrastructure.Handlers.Rating
{
    internal class GetSemifinalistsQueryHandler: AutoMapperEFHandler, IRequestHandler<GetSemifinalistsQuery, IQueryable<RatingSemifinalist>>
    {
        public GetSemifinalistsQueryHandler(AvatarAppContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public Task<IQueryable<RatingSemifinalist>> Handle(GetSemifinalistsQuery request, CancellationToken cancellationToken)
        {
            var query = DbContext.Semifinalists.OrderByDescending(semifinalist => semifinalist.Date).Select(semifinalist => semifinalist.User);
            return Task.FromResult(Mapper.ProjectTo<RatingSemifinalist>(query));
        }
    }
}
