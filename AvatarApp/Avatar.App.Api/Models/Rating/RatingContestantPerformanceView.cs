using Avatar.App.Api.Models.Common;
using Avatar.App.Api.Models.Content;
using Avatar.App.Rating.Models;

namespace Avatar.App.Api.Models.Rating
{
    public class RatingContestantPerformanceView: BaseContestantView
    {
        public RatingContestantPerformanceView(RatingContestantPerformance performance) : base(performance)
        {
            LikesNumber = performance.LikesNumber;
            Video = performance.ActiveVideo != null ? new VideoView(performance.ActiveVideo) : null;
        }

        public int LikesNumber { get; set; }
        public VideoView Video { get; set; }
    }
}
