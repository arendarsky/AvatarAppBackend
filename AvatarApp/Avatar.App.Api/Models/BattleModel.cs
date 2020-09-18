using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Api.Models.UserModels;
using Avatar.App.Core.Entities;

namespace Avatar.App.Api.Models
{
    public class BattleModel
    {
        public long Id { get; set; }
        public DateTime EndDate { get; set; }
        public int WinnersNumber { get; set; }
        public int TotalVotesNumber { get; set; }
        public IEnumerable<BattleParticipantUserModel> BattleParticipants { get; set; }

        public static BattleModel FromBattleWithUserLikeInfo(Battle battle, Guid userGuid)
        {
            return new BattleModel(battle, userGuid);
        }

        private BattleModel(Battle battle, Guid userGuid): this(battle)
        {
            BattleParticipants = battle.BattleSemifinalists.Select(battleSemifinalist =>
                BattleParticipantUserModel.FromSemifinalistWithUserLikeInfo(battleSemifinalist.Semifinalist, userGuid));
        }

        private BattleModel(Battle battle)
        {
            Id = battle.Id;
            EndDate = battle.EndDate;
            WinnersNumber = battle.WinnersNumber;
            TotalVotesNumber = battle.Votes.Count();
        }
    }
}
