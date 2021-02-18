using Avatar.App.Casting.Models;
using Avatar.App.SharedKernel.Models;

namespace Avatar.App.Rating.Models
{
    public class RatingContestantPerformance : ContestantPerformance
    {
        public int LikesNumber { get; set; }
        public bool IsSemifinalist { get; set; }
    }

    public abstract class RatingContestant : BaseContestant
    {
        public int LikesNumber { get; set; }
    }

    public class RatingSemifinalist: RatingContestant
    {

    }

    public class RatingFinalist : RatingContestant
    {

    }
}
