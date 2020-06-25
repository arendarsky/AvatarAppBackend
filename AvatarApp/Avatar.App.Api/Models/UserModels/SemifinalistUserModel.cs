using System;
using Avatar.App.Core.Entities;
using Avatar.App.Core.Models;

namespace Avatar.App.Api.Models.UserModels
{
    public class SemifinalistUserModel:BaseUserModel
    {
        public SemifinalistUserModel(LikedVideo likedVideo) : base(likedVideo.User)
        {
            Date = likedVideo.Date;
        }

        public SemifinalistUserModel(UserProfile user): base(user.User)
        {
            LikesNumber = user.LikesNumber;
        }

        public DateTime Date { get; set; }
        public int LikesNumber { get; set; }
    }
}
