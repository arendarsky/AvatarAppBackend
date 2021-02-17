using Avatar.App.Api.Models.Common;
using Avatar.App.Rating.Models;

namespace Avatar.App.Api.Models.Rating
{
    public class RatingContestantView: BaseContestantView
    {
        public RatingContestantView(RatingContestant user) : base(user)
        {
            LikesNumber = user.LikesNumber;
        }

        public int LikesNumber { get; }
    }
}
