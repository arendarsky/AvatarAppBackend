using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avatar.App.Casting.Commands;
using Avatar.App.Core.Models;
using Avatar.App.Final.Commands;
using Avatar.App.Infrastructure.CommandHandlers.Basic;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Avatar.App.Infrastructure.CommandHandlers.Final
{
    internal class GetFinalistsHandler: EFCommandHandler, IRequestHandler<GetFinalists, IEnumerable<UserProfile>>
    {
        private readonly IMediator _mediator;

        public GetFinalistsHandler(AvatarAppContext dbContext, IMediator mediator) : base(dbContext)
        {
            _mediator = mediator;
        }

        public async Task<IEnumerable<UserProfile>> Handle(GetFinalists request, CancellationToken cancellationToken)
        {
            var finalists = await DbContext.Finalists.Include(finalist => finalist.User).ToListAsync(cancellationToken);
            var tasks = new List<UserProfile>();

            foreach (var finalist in finalists)
            {
                var task = new UserProfile
                {
                    User = finalist.User,
                    LikesNumber = await _mediator.Send(new GetLikesNumber(finalist.UserId), cancellationToken)
                };

                tasks.Add(task);
            }

            return tasks;
        }
    }
}
