using System.Collections.Generic;
using System.Linq;
using Avatar.App.Api.Models.Common;
using Avatar.App.Profile.Models;

namespace Avatar.App.Api.Models.Profile
{
    public abstract class ContestantProfileView: BaseContestantView
    {
        protected ContestantProfileView(ContestantProfile profile) : base(profile)
        {
            InstagramLogin = profile.InstagramLogin;
        }

        public string InstagramLogin { get; set; }
    }

    public class PrivateContestantProfileView : ContestantProfileView
    {
        public PrivateContestantProfileView(PrivateContestantProfile profile) : base(profile)
        {
            LikesNumber = profile.LikesNumber;
            Email = profile.Email;
            Videos = profile.Videos.Select(v => new PrivateProfileVideoView(v)).ToList();
        }

        public string Email { get; set; }
        public int LikesNumber { get; set; }
        public ICollection<PrivateProfileVideoView> Videos { get; set; }
    }

    public class PublicContestantProfileView : ContestantProfileView
    {
        public PublicContestantProfileView(PublicContestantProfile profile) : base(profile)
        {
            Videos = profile.Videos.Select(v => new PublicProfileVideoView(v)).ToList();
        }

        public ICollection<PublicProfileVideoView> Videos { get; set; }
    }
}
