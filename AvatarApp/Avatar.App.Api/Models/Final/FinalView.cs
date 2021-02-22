using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avatar.App.Api.Models.Final
{
    public class FinalView
    {
        public FinalView(App.Final.Models.Final final, long userId)
        {
            VideoUrl = final.VideoUrl;
            IsVotingStarted = final.IsVotingStarted;
            Finalists = final.Finalists.Select(finalist => new FinalistView(finalist, userId));
            WinnersNumber = final.WinnersNumber;
        }

        public string VideoUrl { get; set; }
        public bool IsVotingStarted { get; set; }
        public int WinnersNumber { get; set; }
        public IEnumerable<FinalistView> Finalists { get; set; }
    }
}
