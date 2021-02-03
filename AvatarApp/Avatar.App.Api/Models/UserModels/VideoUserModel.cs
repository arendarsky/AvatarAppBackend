using Avatar.App.Infrastructure.Models.Casting;

namespace Avatar.App.Api.Models.UserModels
{
    public class VideoUserModel: BaseUserModel
    {
        public VideoUserModel(VideoDb video) : base(video.User)
        {
            Video = new VideoModel(video);
        }

        public VideoModel Video { get; set; }
    }
}
