using Avatar.App.Semifinal.Models;

namespace Avatar.App.Api.Models.Semifinal
{
    public class BattleVoteUpdateView
    {
        public int TotalVotesNumber { get; set; }
        public bool IsLikedByUser { get; set; }
        public int VotesNumber { get; set; }

        public BattleVoteUpdateView(VotingRoom votingRoom)
        {
            TotalVotesNumber = votingRoom.BattleVotesNumber;
            IsLikedByUser = votingRoom.IsVoted;
            VotesNumber = votingRoom.SemifinalistVotesNumber;
        }
    }
}
