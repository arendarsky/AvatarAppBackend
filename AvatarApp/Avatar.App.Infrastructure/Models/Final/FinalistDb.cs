﻿using System;
using System.Collections.Generic;
using Avatar.App.Infrastructure.Models.Casting;

namespace Avatar.App.Infrastructure.Models.Final
{
    internal class FinalistDb: BaseEntity
    {
        public long UserId { get; set; }
        public UserDb User { get; set; }
        public DateTime Date { get; set; }

        public ICollection<FinalVoteDb> Votes { get; set; }
    }
}
