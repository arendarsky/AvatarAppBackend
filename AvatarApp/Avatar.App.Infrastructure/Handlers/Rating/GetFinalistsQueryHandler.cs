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
    internal class GetFinalistsQueryHandler: AutoMapperEFHandler, IRequestHandler<GetFinalistsQuery, IQueryable<RatingFinalist>>

    {
        public GetFinalistsQueryHandler(AvatarAppContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public Task<IQueryable<RatingFinalist>> Handle(GetFinalistsQuery request, CancellationToken cancellationToken)
        {
            var query = DbContext.Finalists.OrderByDescending(finalist => finalist.Date).Select(finalist => finalist.User);
            return Task.FromResult(Mapper.ProjectTo<RatingFinalist>(query));
        }
    }
}
