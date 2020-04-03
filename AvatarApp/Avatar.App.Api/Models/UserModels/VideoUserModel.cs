using System.Linq;
using Avatar.App.Core.Entities;

namespace Avatar.App.Api.Models.UserModels
{
    public class VideoUserModel: BaseUserModel
    {
        public VideoUserModel(Video video) : base(video.User)
        {
            Video = new VideoModel(video);
        }

        public VideoModel Video { get; set; }
    }
}
