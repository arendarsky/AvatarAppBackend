using System.Collections.Generic;
using System.Threading.Tasks;
using Avatar.App.Semifinal.CData;
using Avatar.App.Semifinal.Commands;
using Avatar.App.Semifinal.Models;
using Avatar.App.SharedKernel;
using Avatar.App.SharedKernel.Commands;
using MediatR;

namespace Avatar.App.Semifinal
{
    internal class SemifinalComponent: AvatarAppComponent, ISemifinalComponent
    {
        public SemifinalComponent(IMediator mediator, IQueryManager queryManager) : base(mediator, queryManager)
        {
        }

        public async Task AddBattle(BattleCData battleDTO)
        {
            await Mediator.Send(new AddBattle(battleDTO));
        }

        public async Task<VotingRoom> VoteTo(BattleVoteCData voteCData)
        {
            var battle = await Mediator.Send(new GetById<Battle>(voteCData.BattleId));
            var votingRoom = new VotingRoom(battle, voteCData);
            await Mediator.Send(new UpdateVotes(votingRoom));
            return votingRoom;
        }

        public async Task<IEnumerable<Battle>> GetBattles()
        {
            var query = await Mediator.Send(new GetQuery<Battle>());
            return await QueryManager.ToListAsync(query);
        }
    }
}
