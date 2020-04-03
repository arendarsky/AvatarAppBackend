using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Core.Entities;

namespace Avatar.App.Api.Models.UserModels
{
    public class ModerationUserModel: BaseUserModel
    {
        public ModerationUserModel(Video video) : base(video.User)
        {
            Video = new VideoModel(video);
            Email = video.User.Email;
        }

        public VideoModel Video { get; set; }
        public string Email { get; set; }
    }
}
