using Avatar.App.Core.Entities;

namespace Avatar.App.Api.Models
{
    public class VideoModel
    {

        public VideoModel(Video video)
        {
            Name = video.Name;
            StartTime = video.StartTime;
            EndTime = video.EndTime;
            IsActive = video.IsActive;
            IsApproved = video.IsApproved;
        }

        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool? IsApproved { get; set; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }
    }
}
