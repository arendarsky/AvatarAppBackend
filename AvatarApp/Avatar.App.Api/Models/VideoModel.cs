namespace Avatar.App.Api.Models
{
    public class VideoModel
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool? IsApproved { get; set; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }
    }
}
