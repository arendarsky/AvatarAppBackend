using System;
using System.Collections.Generic;
using System.Linq;
using Avatar.App.Api.Models.UserModels;
using Avatar.App.Semifinal.Models;

namespace Avatar.App.Api.Models
{
    public class BattleModel
    {
        public long Id { get; set; }
        public DateTime EndDate { get; set; }
        public long SecondsUntilTheEnd { get; set; }
        public int WinnersNumber { get; set; }
        public int TotalVotesNumber { get; set; }
        public IEnumerable<BattleParticipantUserModel> BattleParticipants { get; set; }

        public static BattleModel FromBattleWithUserLikeInfo(Battle battle, Guid userGuid)
        {
            return new BattleModel(battle, userGuid);
        }

        private BattleModel(Battle battle, Guid userGuid): this(battle)
        {
            BattleParticipants = battle.Participants.Select(participant =>
                BattleParticipantUserModel.FromSemifinalistWithUserLikeInfo(participant, userGuid, battle.Id));
        }

        private BattleModel(Battle battle)
        {
            Id = battle.Id;
            EndDate = battle.EndDate;
            var timeNow = DateTime.Now;
            SecondsUntilTheEnd = battle.EndDate > timeNow ? (battle.EndDate - timeNow).Seconds : 0;
            WinnersNumber = battle.WinnersNumber;
            TotalVotesNumber = battle.Participants
                .SelectMany(participant => participant.Votes.Where(vote => vote.Battle.Id == battle.Id)).Count();
        }
    }
}
