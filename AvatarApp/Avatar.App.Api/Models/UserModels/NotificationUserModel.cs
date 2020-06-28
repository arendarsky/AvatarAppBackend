using System;
using Avatar.App.Core.Entities;

namespace Avatar.App.Api.Models.UserModels
{
    public class NotificationUserModel:BaseUserModel
    {
        public NotificationUserModel(LikedVideo likedVideo) : base(likedVideo.User)
        {
            Date = likedVideo.Date;
        }

        public DateTime Date { get; set; }
    }
}
