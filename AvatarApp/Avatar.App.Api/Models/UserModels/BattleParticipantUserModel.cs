﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Core.Entities;

namespace Avatar.App.Api.Models.UserModels
{
    public class BattleParticipantUserModel: BaseUserModel
    {
        public SemifinalistModel Semifinalist { get; set; }

        public static BattleParticipantUserModel FromSemifinalistWithUserLikeInfo(Semifinalist semifinalist, Guid userGuid)
        {
            return new BattleParticipantUserModel(semifinalist, userGuid);
        }

        private BattleParticipantUserModel(Semifinalist semifinalist, Guid userGuid) : base(semifinalist.User)
        {
            Semifinalist = SemifinalistModel.FromSemifinalistWithUserLikeInfo(semifinalist, userGuid);
        }
    }
}