using Avatar.App.Casting.Models;

namespace Avatar.App.Api.Models.Content
{
    public class VideoView
    {
        public VideoView(Video video)
        {
            Name = video.Name;
            StartTime = video.StartTime;
            EndTime = video.EndTime;
        }

        public string Name { get; set; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }
    }
}
