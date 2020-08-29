using System;
using System.Collections.Generic;
using System.Text;

namespace Avatar.App.Core.Models
{
    public class BattleCreatingDto
    {
        public IEnumerable<long> SemifinalistsId { get; set; }

        public DateTime EndDate { get; set; }
    }
}
