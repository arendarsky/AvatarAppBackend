using System.Linq;
using Avatar.App.Core.Entities;

namespace Avatar.App.Api.Models.UserModels
{
    public class CastingUserModel: BaseUserModel
    {
        public CastingUserModel(User user) : base(user)
        {
            var video = user.LoadedVideos.FirstOrDefault(v => v.IsActive);

            if (video == null) return;

            Video = new VideoModel(video);
        }

        public VideoModel Video { get; set; }
    }
}
