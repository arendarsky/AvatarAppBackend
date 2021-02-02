using System.Threading;
using System.Threading.Tasks;
using Avatar.App.Core.Entities;
using Avatar.App.Infrastructure.CommandHandlers.Basic;
using Avatar.App.Infrastructure.Models.Semifinal;
using Avatar.App.Semifinal.Commands;
using MediatR;

namespace Avatar.App.Infrastructure.CommandHandlers.Semifinal
{
    internal class CloseBattleHandler: EFCommandHandler, IRequestHandler<CloseBattle>
    {
        public CloseBattleHandler(AvatarAppContext dbContext) : base(dbContext)
        {
        }

        public async Task<Unit> Handle(CloseBattle request, CancellationToken cancellationToken)
        {
            var battle = new BattleDb{Id = request.BattleId};
            var property = DbContext.Entry(battle).Property(bat => bat.Closed);
            property.CurrentValue = true;
            property.IsModified = true;
            await DbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
