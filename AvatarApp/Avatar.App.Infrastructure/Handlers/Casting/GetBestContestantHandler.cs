using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Avatar.App.Casting.Commands;
using Avatar.App.Infrastructure.Handlers.Abstract;
using Avatar.App.Rating.Models;
using Avatar.App.SharedKernel.Commands;
using Avatar.App.SharedKernel.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Avatar.App.Infrastructure.Handlers.Casting
{
    internal class GetBestContestantHandler: AutoMapperEFHandler, IRequestHandler<GetBestContestant, BaseContestant>
    {
        private readonly IMediator _mediator;

        public GetBestContestantHandler(AvatarAppContext dbContext, IMapper mapper, IMediator mediator) : base(dbContext, mapper)
        {
            _mediator = mediator;
        }

        public async Task<BaseContestant> Handle(GetBestContestant request, CancellationToken cancellationToken)
        {
            var query = await _mediator.Send(new GetQuery<RatingContestantPerformance>(), cancellationToken);
            var rating = await query.ToListAsync(cancellationToken);
            return rating.OrderByDescending(contestant => contestant.LikesNumber).FirstOrDefault();
        }
    }
}
