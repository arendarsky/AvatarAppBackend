using System.Collections.Generic;
using System.Linq;
using Avatar.App.Core.Entities;

namespace Avatar.App.Api.Models.UserModels
{
    public class PublicProfileUserModel: BaseUserModel
    {
        public PublicProfileUserModel(User user) : base(user)
        {
            InstagramLogin = user.InstagramLogin;

            if (user.LoadedVideos == null) return;

            Videos = user.LoadedVideos.Where(c => c.IsApproved.HasValue && c.IsApproved == true)
                .Select(v => new VideoModel(v)).ToList();
        }
        public string InstagramLogin { get; set; }
        public ICollection<VideoModel> Videos { get; set; }
    }
}
