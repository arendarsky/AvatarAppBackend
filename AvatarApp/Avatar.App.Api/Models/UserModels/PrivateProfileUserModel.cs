using System.Collections.Generic;
using System.Linq;
using Avatar.App.Core.Entities;

namespace Avatar.App.Api.Models.UserModels
{
    public class PrivateProfileUserModel: BaseUserModel
    {
        public PrivateProfileUserModel(User user, int likesNumber) : base(user)
        {
            LikesNumber = likesNumber;
            Email = user.Email;
            InstagramLogin = user.InstagramLogin;

            if (user.LoadedVideos == null) return;

            Videos = user.LoadedVideos.Select(v => new VideoModel(v)).ToList();
        }

        public int LikesNumber { get; set; }
        public string InstagramLogin { get; set; }
        public string Email { get; set; }
        public ICollection<VideoModel> Videos { get; set; }
    }
}
