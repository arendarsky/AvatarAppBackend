using System;

namespace Avatar.App.Casting.Models
{
    public class BaseContestant
    {
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProfilePhoto { get; set; }
    }

    public class Contestant: BaseContestant
    {
        public int VideosNumber { get; set; }
    }

    public class ContestantPerformance : BaseContestant
    {
        public Video ActiveVideo { get; set; }
    }
}
