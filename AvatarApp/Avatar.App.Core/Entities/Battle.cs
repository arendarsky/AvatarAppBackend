using Avatar.App.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Avatar.App.Core.Entities
{
    public class Battle : BaseEntity
    {
        public DateTime? CreationDate { get; set; }

        public DateTime? EndDate { get; set; } 

        public ICollection<LikedVideo> Likes { get; set; }

        public ICollection<BattleSemifinalist> BattleSemifinalists { get; set; }
    }
}
