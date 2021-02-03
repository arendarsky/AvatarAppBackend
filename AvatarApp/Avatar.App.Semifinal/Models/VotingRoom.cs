using System.Linq;
using Avatar.App.Semifinal.CData;

namespace Avatar.App.Semifinal.Models
{
    public sealed class VotingRoom
    {
        private readonly Battle _battle;
        private readonly Semifinalist _semifinalist;
        private readonly BattleVoteCData _voteCData;
        
        public BattleVote VoteToRemove { get; set; }
        public BattleVoteCData VoteToAdd{ get; set; }
        public int BattleVotesNumber => _battle.TotalVotesNumber;
        public int SemifinalistVotesNumber => _semifinalist.GetBattleVotes(_battle.Id).Count();
        public bool IsVoted => VoteToAdd != null || VoteToRemove == null;

        public VotingRoom(Battle battle, BattleVoteCData voteCData)
        {
            _battle = battle;
            _semifinalist = _battle.Participants.First(participant => participant.Id == voteCData.SemifinalistId);
            _voteCData = voteCData;
            VoteTo();
        }

        private void VoteTo()
        {
            var existedVote = _semifinalist.GetUserVote(_voteCData);

            if (existedVote != null)
            {
                _semifinalist.Votes.Remove(existedVote);
                VoteToRemove = existedVote;
                return;
            }

            if (_battle.IsVoteLimitExpired(_voteCData.UserId))
            {
                return;
            }

            _semifinalist.Votes.Add((BattleVote) _voteCData);
            VoteToAdd = _voteCData;
        }
    }
}
