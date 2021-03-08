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
            var datetimeNow = DateTime.Now;
            var secondsUntilStart = final.VotingStartTime.HasValue ? (final.VotingStartTime.Value - datetimeNow).TotalSeconds: 0;
            var secondsUntilEnd = final.VotingEndTime.HasValue ? (final.VotingEndTime.Value - datetimeNow).TotalSeconds : 0;
            VideoUrl = final.VideoUrl;
            SecondsUntilEnd = secondsUntilEnd >= 0 ? (int) secondsUntilEnd : 0;
            SecondsUntilStart = secondsUntilStart >= 0 ? (int) secondsUntilStart : 0;
            Finalists = final.Finalists.Select(finalist => new FinalistView(finalist, userId));
            WinnersNumber = final.WinnersNumber;
        }


        public string VideoUrl { get; set; }
        public int SecondsUntilStart { get; set; }
        public int SecondsUntilEnd { get; set; }
        public int WinnersNumber { get; set; }
        public IEnumerable<FinalistView> Finalists { get; set; }
    }
}
