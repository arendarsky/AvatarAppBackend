using System;
using System.Linq;
using Avatar.App.Semifinal.Models;

namespace Avatar.App.Api.Models
{
    public class SemifinalistModel
    {
        public long Id { get; set; }
        public string VideoName { get; set; }
        public int VotesNumber { get; set; }
        public bool IsLikedByUser { get; set; }
        public bool IsFinalist { get; set; }

        public static SemifinalistModel FromSemifinalistWithUserLikeInfo(Semifinalist semifinalist, Guid userGuid, long battleId)
        {
            return new SemifinalistModel(semifinalist, userGuid, battleId);
        }

        private SemifinalistModel(Semifinalist semifinalist, Guid userGuid, long battleId) : this(semifinalist, battleId)
        {
            IsLikedByUser = semifinalist.Votes?.Any(vote => vote.BattleId == battleId && vote.From.Guid == userGuid) ?? false;
        }

        private SemifinalistModel(Semifinalist semifinalist, long battleId)
        {
            Id = semifinalist.Id;
            VideoName = semifinalist.VideoName;
            VotesNumber = semifinalist.Votes?.Count(vote => vote.Battle.Id == battleId) ?? 0;
            IsFinalist = semifinalist.IsFinalist;
        }
    }
}
