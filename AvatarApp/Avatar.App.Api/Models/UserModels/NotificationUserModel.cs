using System;
using Avatar.App.Infrastructure.Models.Casting;

namespace Avatar.App.Api.Models.UserModels
{
    public class NotificationUserModel:BaseUserModel
    {
        public NotificationUserModel(LikedVideoDb likedVideo) : base(likedVideo.User)
        {
            Date = likedVideo.Date;
        }

        public DateTime Date { get; set; }
    }
}
