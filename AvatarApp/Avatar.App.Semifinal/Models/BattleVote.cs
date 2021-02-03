using System;
using Avatar.App.Semifinal.CData;

namespace Avatar.App.Semifinal.Models
{
    public class BattleVote: BattleVoteCData
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
    }
}
