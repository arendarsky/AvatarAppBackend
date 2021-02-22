using System;
using System.Collections.Generic;
using System.Text;
using Avatar.App.SharedKernel.Models;

namespace Avatar.App.Final.Models
{
    public class Finalist
    {
        public long Id { get; set; }
        public BaseContestant Contestant { get; set; }
        public IEnumerable<FinalVote> Votes { get; set; }
    }
}
