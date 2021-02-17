namespace Avatar.App.Profile.Models
{
    public abstract class ProfileVideo
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }
    }

    public class PublicProfileVideo: ProfileVideo
    {

    }

    public class PrivateProfileVideo : ProfileVideo
    {
        public bool? IsApproved { get; set; }
    }
}
