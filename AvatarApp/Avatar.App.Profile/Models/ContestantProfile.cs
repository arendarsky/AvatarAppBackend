using System.Collections.Generic;
using Avatar.App.Casting.Models;

namespace Avatar.App.Profile.Models
{
    public abstract class ContestantProfile: BaseContestant
    {
        public string InstagramLogin { get; set; }
    }

    public class PrivateContestantProfile: ContestantProfile
    {
        public int LikesNumber { get; set; }
        public string Email { get; set; }
        public IEnumerable<PrivateProfileVideo> Videos { get; set; }
    }

    public class PublicContestantProfile : ContestantProfile
    {
        public IEnumerable<PublicProfileVideo> Videos { get; set; }
    }
}
