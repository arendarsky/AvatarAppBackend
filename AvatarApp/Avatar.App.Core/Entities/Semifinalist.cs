using System;
using System.Collections.Generic;
using System.Text;
using Avatar.App.SharedKernel;

namespace Avatar.App.Core.Entities
{
    public class Semifinalist: BaseEntity
    {
        public long UserId { get; set; }

        public User User { get; set; }

        public DateTime? Date { get; set; }

        public Video Video { get; set; }

        public ICollection<BattleSemifinalist> BattleSemifinalists { get; set; }
    }
}
