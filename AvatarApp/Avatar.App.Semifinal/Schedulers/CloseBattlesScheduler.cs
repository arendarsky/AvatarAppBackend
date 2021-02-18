using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Semifinal.Commands;
using Avatar.App.Semifinal.Models;
using Avatar.App.SharedKernel;
using MediatR;

namespace Avatar.App.Semifinal.Schedulers
{
    internal class CloseBattlesScheduler: ICronInvocable
    {
        private readonly IMediator _mediator;

        public CloseBattlesScheduler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Invoke()
        {
            var battles = await _mediator.Send(new GetExpiredBattles());

            foreach (var battle in battles)
            {
                await CloseBattle(battle);
            }
        }

        private async Task CloseBattle(Battle battle)
        {
            await _mediator.Send(new CloseBattle(battle.Id));
            var participants = battle.Participants;
            var winners = participants.OrderByDescending(participant => participant.Votes.Count())
                .Take(battle.WinnersNumber);
            await _mediator.Send(new AddFinalists(winners));
        }
    }
}
