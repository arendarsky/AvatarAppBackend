using System;
using System.Collections.Generic;
using System.Text;

namespace Avatar.App.Final.CreationData
{
    public class FinalVoteCreation
    {
        public FinalVoteCreation(long userId, long finalistId)
        {
            UserId = userId;
            FinalistId = finalistId;
        }

        public long FinalistId { get; }
        public long UserId { get; }
    }
}
