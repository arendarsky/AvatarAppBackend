using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avatar.App.Infrastructure.Handlers.Abstract;
using Avatar.App.Infrastructure.Models.Final;
using Avatar.App.Semifinal.Commands;
using MediatR;

namespace Avatar.App.Infrastructure.Handlers.Final
{
    internal class AddFinalistsHandler: EFHandler, IRequestHandler<AddFinalists>
    {
        public AddFinalistsHandler(AvatarAppContext dbContext) : base(dbContext)
        {
        }

        public async Task<Unit> Handle(AddFinalists request, CancellationToken cancellationToken)
        {
            var finalists = request.Semifinalists.Select(semifinalist => new FinalistDb
            {
                UserId = semifinalist.Contestant.Id,
                Date = DateTime.Now
            });

            await DbContext.Finalists.AddRangeAsync(finalists, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
