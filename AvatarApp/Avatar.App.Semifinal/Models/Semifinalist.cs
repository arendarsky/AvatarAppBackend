using System.Collections.Generic;
using System.Linq;
using Avatar.App.Casting.Models;
using Avatar.App.Semifinal.CData;

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
            return GetUserVote(battleVoteCData.UserId, battleVoteCData.BattleId);
        } 

        private BattleVote GetUserVote(long userId, long battleId)
        {
            return GetBattleVotes(battleId).FirstOrDefault(vote => vote.UserId == userId);
        }
    }
}
