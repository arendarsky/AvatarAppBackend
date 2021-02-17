using Avatar.App.Administration.Models;
using Avatar.App.Api.Models.Common;
using Avatar.App.Api.Models.Content;

namespace Avatar.App.Api.Models.Administration
{
    public class ContestantModerationView: BaseContestantView
    {
        public ContestantModerationView(ModerationContestantPerformance performance) : base(performance)
        {
            Video = new VideoView(performance.ActiveVideo);
            Email = performance.Email;
        }

        public VideoView Video { get; set; }
        public string Email { get; set; }
    }
}
