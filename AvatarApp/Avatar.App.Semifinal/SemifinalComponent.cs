using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Internal;
using Avatar.App.Semifinal.Commands;
using Avatar.App.Semifinal.Models;
using MediatR;

namespace Avatar.App.Semifinal
{
    internal class SemifinalComponent: ISemifinalComponent
    {
        private readonly IMediator _mediator;
        private readonly IFinalistsSetter _finalComponent;

        public SemifinalComponent(IMediator mediator, IFinalistsSetter finalComponent)
        {
            _mediator = mediator;
            _finalComponent = finalComponent;
        }

        public async Task CloseBattlesAsync()
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
            await _finalComponent.AddFinalists(winners);
        }
        public async Task<IEnumerable<Battle>> GetBattles()
        {
            return await _mediator.Send(new GetBattles());
        }
    }
}
