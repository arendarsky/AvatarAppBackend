using System;
using System.Collections.Generic;
using System.Text;
using Avatar.App.Casting.Models;

namespace Avatar.App.Semifinal.Models
{
    public class Semifinalist
    {
        public long Id { get; set; }
        public string VideoName { get; set; }
        public bool IsFinalist { get; set; }

        public Contestant Contestant { get; set; }
        public IEnumerable<Contestant> Votes { get; set; } 
    }
}
