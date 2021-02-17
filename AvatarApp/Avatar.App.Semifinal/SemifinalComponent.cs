using System.Collections.Generic;
using System.Threading.Tasks;
using Avatar.App.Semifinal.CData;
using Avatar.App.Semifinal.Commands;
using Avatar.App.Semifinal.Models;
using Avatar.App.SharedKernel.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Avatar.App.Semifinal
{
    internal class SemifinalComponent: ISemifinalComponent
    {
        private readonly IMediator _mediator;

        public SemifinalComponent(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task AddBattle(BattleCData battleDTO)
        {
            await _mediator.Send(new AddBattle(battleDTO));
        }

        public async Task<VotingRoom> VoteTo(BattleVoteCData voteCData)
        {
            var battle = await _mediator.Send(new GetById<Battle>(voteCData.BattleId));
            var votingRoom = new VotingRoom(battle, voteCData);
            await _mediator.Send(new UpdateVotes(votingRoom));
            return votingRoom;
        }

        public async Task<IEnumerable<Battle>> GetBattles()
        {
            var query = await _mediator.Send(new GetQuery<Battle>());
            return await query.ToListAsync();
        }
    }
}
