using System;
using System.Collections.Generic;
using System.Text;

namespace Avatar.App.Infrastructure.Models.Final
{
    internal class FinalDb: BaseEntity
    {
        public string VideoUrl { get; set; }
        public bool IsActive { get; set; }
        public bool IsVotingStarted { get; set; }
        public int WinnersNumber { get; set; }
    }
}
