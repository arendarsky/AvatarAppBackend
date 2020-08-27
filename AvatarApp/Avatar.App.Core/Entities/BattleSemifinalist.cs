using System;
using System.Collections.Generic;
using System.Text;
using Avatar.App.SharedKernel;

namespace Avatar.App.Core.Entities
{
    public class BattleSemifinalist: BaseEntity
    {
        public long BattleId { get; set; }

        public Battle Battle { get; set; }

        public long SemifinalistId { get; set; }

        public Semifinalist Semifinalist { get; set; }
    }
}
