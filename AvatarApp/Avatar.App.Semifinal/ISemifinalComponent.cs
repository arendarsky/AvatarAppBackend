using System.Collections.Generic;
using System.Threading.Tasks;
using Avatar.App.Semifinal.CData;
using Avatar.App.Semifinal.Models;

namespace Avatar.App.Semifinal
{
    public interface ISemifinalComponent
    {
        Task AddBattle(BattleCData battleDTO);
        Task<VotingRoom> VoteTo(BattleVoteCData voteDTO);
        Task<IEnumerable<Battle>> GetBattles();
    }
}
