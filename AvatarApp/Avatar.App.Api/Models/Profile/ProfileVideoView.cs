using Avatar.App.Profile.Models;

namespace Avatar.App.Api.Models.Profile
{
    public abstract class ProfileVideoView
    {
        protected ProfileVideoView(ProfileVideo video)
        {
            Name = video.Name;
            StartTime = video.StartTime;
            EndTime = video.EndTime;
            IsActive = video.IsActive;
        }

        public string Name { get; set; }
        public bool IsActive { get; set; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }
    }

    public class PublicProfileVideoView : ProfileVideoView
    {
        public PublicProfileVideoView(PublicProfileVideo video) : base(video)
        {
        }
    }

    public class PrivateProfileVideoView: ProfileVideoView
    {

        public PrivateProfileVideoView(PrivateProfileVideo video): base(video)
        {
            IsApproved = video.IsApproved;
        }

        public bool? IsApproved { get; set; }
    }
}
