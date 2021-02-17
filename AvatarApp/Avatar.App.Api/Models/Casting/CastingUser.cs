using Avatar.App.Api.Models.Common;
using Avatar.App.Api.Models.Content;
using Avatar.App.Casting.Models;

namespace Avatar.App.Api.Models.Casting
{
    public class CastingUser: BaseContestantView
    {
        public CastingUser(ContestantPerformance performance) : base(performance)
        {
            Video = new VideoView(performance.ActiveVideo);
        }

        public VideoView Video { get; set; }
    }
}
