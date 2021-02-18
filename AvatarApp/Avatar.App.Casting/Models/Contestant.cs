using Avatar.App.SharedKernel.Models;

namespace Avatar.App.Casting.Models
{
    public class Contestant: BaseContestant
    {
        public int VideosNumber { get; set; }
    }

    public class ContestantPerformance : BaseContestant
    {
        public Video ActiveVideo { get; set; }
    }
}
