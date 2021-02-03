using Avatar.App.Semifinal.Models;

namespace Avatar.App.Api.Models
{
    public class BattleParticipantsVote
    {
        public int TotalVotesNumber { get; set; }
        public bool IsLikedByUser { get; set; }
        public int VotesNumber { get; set; }

        public BattleParticipantsVote(VotingRoom votingRoom)
        {
            TotalVotesNumber = votingRoom.BattleVotesNumber;
            IsLikedByUser = votingRoom.IsVoted;
            VotesNumber = votingRoom.SemifinalistVotesNumber;
        }
    }
}
