using System;
using System.Collections.Generic;

namespace Avatar.App.Semifinal.CData
{
    public class BattleCData
    {
        public IEnumerable<long> SemifinalistsId { get; set; }
        public DateTime EndDate { get; set; }
        public int WinnersNumber { get; set; }
    }
}
