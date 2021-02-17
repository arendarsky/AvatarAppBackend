using System.Linq;
using Avatar.App.Semifinal.Models;

namespace Avatar.App.Api.Models.Semifinal
{
    public class SemifinalistView
    {
        public long Id { get; set; }
        public string VideoName { get; set; }
        public int VotesNumber { get; set; }
        public bool IsLikedByUser { get; set; }
        public bool IsFinalist { get; set; }

        public SemifinalistView(Semifinalist semifinalist, long userId, long battleId) : this(semifinalist, battleId)
        {
            IsLikedByUser = semifinalist.Votes.Any(vote => vote.BattleId == battleId && vote.UserId == userId);
        }

        public SemifinalistView(Semifinalist semifinalist, long battleId)
        {
            Id = semifinalist.Id;
            VideoName = semifinalist.VideoName;
            VotesNumber = semifinalist.GetBattleVotes(battleId).Count();
            IsFinalist = semifinalist.IsFinalist;
        }
    }
}
