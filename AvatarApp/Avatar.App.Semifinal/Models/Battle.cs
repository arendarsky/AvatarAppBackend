using System;
using System.Collections.Generic;
using System.Text;

namespace Avatar.App.Semifinal.Models
{
    public class Battle
    {
        public long Id { get; set; }
        public DateTime EndDate { get; set; }
        public int WinnersNumber { get; set; }
        public IEnumerable<Semifinalist> Participants { get; set; }
    }
}
