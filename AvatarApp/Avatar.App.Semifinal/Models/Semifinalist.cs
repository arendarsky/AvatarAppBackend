using System.Collections.Generic;
using System.Linq;
using Avatar.App.Semifinal.CData;
using Avatar.App.SharedKernel.Models;

namespace Avatar.App.Semifinal.Models
{
    public class Semifinalist
    {
        public long Id { get; set; }
        public string VideoName { get; set; }
        public bool IsFinalist { get; set; }
        
        public BaseContestant Contestant { get; set; } 
        public ICollection<BattleVote> Votes { get; set; }

        public IEnumerable<BattleVote> GetBattleVotes(long battleId)
        {
            return Votes.Where(vote => vote.BattleId == battleId);
        }

        public BattleVote GetUserVote(BattleVoteCData battleVoteCData)
        {
            return battleVoteCData.UserId.HasValue
                ? GetUserVote(battleVoteCData.UserId.Value, battleVoteCData.BattleId)
                : null;
        } 

        private BattleVote GetUserVote(long userId, long battleId)
        {
            return GetBattleVotes(battleId).FirstOrDefault(vote => vote.UserId == userId);
        }
    }
}
