using System;
using System.Collections.Generic;
using System.Text;

namespace Avatar.App.Final.Models
{
    public class Final
    {
        public string VideoUrl { get; set; }
        public bool IsVotingStarted { get; set; }
        public int WinnersNumber { get; set; }
        public IEnumerable<Finalist> Finalists { get; set; }
    }
}
