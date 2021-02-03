using System;
using Avatar.App.Semifinal.Models;

namespace Avatar.App.Api.Models.UserModels
{
    public class BattleParticipantUserModel: BaseUserModel
    {
        public SemifinalistModel Semifinalist { get; set; }

        public static BattleParticipantUserModel FromSemifinalistWithUserLikeInfo(Semifinalist semifinalist, Guid userGuid, long battleId)
        {
            return new BattleParticipantUserModel(semifinalist, userGuid, battleId);
        }

        private BattleParticipantUserModel(Semifinalist semifinalist, Guid userGuid, long battleId) : base(semifinalist.Contestant)
        {
            Semifinalist = SemifinalistModel.FromSemifinalistWithUserLikeInfo(semifinalist, userGuid, battleId);
        }
    }
}
