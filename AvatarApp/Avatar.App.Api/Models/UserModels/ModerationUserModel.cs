using Avatar.App.Infrastructure.Models.Casting;

namespace Avatar.App.Api.Models.UserModels
{
    public class ModerationUserModel: BaseUserModel
    {
        public ModerationUserModel(VideoDb video) : base(video.User)
        {
            Video = new VideoModel(video);
            Email = video.User.Email;
        }

        public VideoModel Video { get; set; }
        public string Email { get; set; }
    }
}
