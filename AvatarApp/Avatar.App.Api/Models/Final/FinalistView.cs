using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Api.Models.Common;
using Avatar.App.Final.Models;

namespace Avatar.App.Api.Models.Final
{
    public class FinalistView
    {
        public FinalistView(Finalist finalist, long userId)
        {
            Id = finalist.Id;
            Contestant = new BaseContestantView(finalist.Contestant);
            IsVotedByUser = finalist.Votes.Any(vote => vote.UserId == userId);
        }

        public long Id { get; set; }
        public bool IsVotedByUser { get; set; }
        public BaseContestantView Contestant { get; set; }
    }
}
