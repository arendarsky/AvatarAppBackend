using System;
using System.Collections.Generic;
using Avatar.App.Core.Entities;
using Avatar.App.SharedKernel;

namespace Avatar.App.Infrastructure.Models.Semifinal
{
    internal class SemifinalistDb: BaseEntity
    {
        public long UserId { get; set; }
        public DateTime Date { get; set; }
        public string VideoName { get; set; }
        public bool IsFinalist { get; set; }

        public User User { get; set; }
        public ICollection<BattleVoteDb> Votes { get; set; }
        public ICollection<BattleSemifinalistDb> BattleSemifinalists { get; set; }
    }
}
