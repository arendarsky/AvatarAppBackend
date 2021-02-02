using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avatar.App.Core.Entities;
using Avatar.App.Final.Commands;
using Avatar.App.Infrastructure.CommandHandlers.Basic;
using Avatar.App.Infrastructure.Models;
using Avatar.App.Infrastructure.Models.Final;
using Avatar.App.Infrastructure.Models.Semifinal;
using MediatR;

namespace Avatar.App.Infrastructure.CommandHandlers.Final
{
    internal class AddFinalistsHandler: EFCommandHandler, IRequestHandler<AddFinalists>
    {
        public AddFinalistsHandler(AvatarAppContext dbContext) : base(dbContext)
        {
        }

        public async Task<Unit> Handle(AddFinalists request, CancellationToken cancellationToken)
        {
            foreach (var semifinalist in request.Semifinalists)
            {
                var semifinalistDb = new SemifinalistDb {Id = semifinalist.Id};
                var property = DbContext.Entry(semifinalistDb).Property(sem => sem.IsFinalist);
                property.CurrentValue = true;
                property.IsModified = true;
            }

            var finalists = request.Semifinalists.Select(semifinalist => new FinalistDb
            {
                UserId = semifinalist.Contestant.Id
            });

            await DbContext.Finalists.AddRangeAsync(finalists, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
