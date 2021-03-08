using System;
using System.Collections.Generic;
using System.Linq;
using Avatar.App.Semifinal.Models;

namespace Avatar.App.Api.Models.Semifinal
{
    public class BattleView
    {
        public long Id { get; set; }
        public DateTime EndDate { get; set; }
        public long SecondsUntilTheEnd { get; set; }
        public int WinnersNumber { get; set; }
        public int TotalVotesNumber { get; set; }
        public IEnumerable<BattleParticipantView> BattleParticipants { get; set; }

        public BattleView(Battle battle, long userId): this(battle)
        {
            BattleParticipants = battle.Participants.Select(participant =>
                new BattleParticipantView(participant, userId, battle.Id));
        }

        private BattleView(Battle battle)
        {
            Id = battle.Id;
            EndDate = battle.EndDate;
            var timeNow = DateTime.Now;
            SecondsUntilTheEnd = battle.EndDate > timeNow ? (int) (battle.EndDate - timeNow).TotalSeconds : 0;
            WinnersNumber = battle.WinnersNumber;
            TotalVotesNumber = battle.TotalVotesNumber;
        }
    }
}
