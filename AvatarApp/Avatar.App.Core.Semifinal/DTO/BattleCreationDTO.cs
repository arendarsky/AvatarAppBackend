using System;
using System.Collections.Generic;

namespace Avatar.App.Core.Semifinal.DTO
{
    public class BattleCreationDTO
    {
        public IEnumerable<long> SemifinalistsId { get; set; }
        public DateTime EndDate { get; set; }
        public int WinnersNumber { get; set; }
    }
}
