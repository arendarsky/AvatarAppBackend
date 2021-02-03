using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avatar.App.Infrastructure.Handlers.Abstract;
using Avatar.App.Infrastructure.Models.Semifinal;
using Avatar.App.Semifinal.Commands;
using MediatR;

namespace Avatar.App.Infrastructure.Handlers.Semifinal
{
    internal sealed class AddBattleHandler: EFHandler, IRequestHandler<AddBattle>
    {
        public AddBattleHandler(AvatarAppContext dbContext) : base(dbContext)
        {
        }

        public async Task<Unit> Handle(AddBattle request, CancellationToken cancellationToken)
        {
            var battleDTO = request.BattleDTO;

            var battleDb = new BattleDb
            {
                WinnersNumber = battleDTO.WinnersNumber,
                EndDate = battleDTO.EndDate,
                CreationDate = DateTime.Now,
                BattleSemifinalists = battleDTO.SemifinalistsId.Select(semifinalistId => new BattleSemifinalistDb
                {
                    SemifinalistId = semifinalistId
                }).ToList()
            };

            await DbContext.Battles.AddAsync(battleDb, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
