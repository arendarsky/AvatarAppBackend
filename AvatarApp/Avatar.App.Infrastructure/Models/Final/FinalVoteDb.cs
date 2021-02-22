using System;
using System.Collections.Generic;
using System.Text;
using Avatar.App.Infrastructure.Models.Casting;

namespace Avatar.App.Infrastructure.Models.Final
{
    internal class FinalVoteDb: BaseEntity
    {
        public long UserId { get; set; }
        public long FinalistId { get; set; }
        public UserDb User { get; set; }
        public FinalistDb Finalist { get; set; }
    }
}
