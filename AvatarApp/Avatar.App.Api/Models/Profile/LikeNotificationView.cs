using System;
using Avatar.App.Communications.Models;

namespace Avatar.App.Api.Models.Profile
{
    public class LikeNotificationView
    {
        public LikeNotificationView(LikeNotification likedVideo)
        {
            Date = likedVideo.Date;
            Id = likedVideo.Author.Id;
            Name = likedVideo.Author.Name;
            ProfilePhoto = likedVideo.Author.ProfilePhoto;
        }

        public long Id { get; }
        public string Name { get; }
        public string ProfilePhoto { get; }
        public DateTime Date { get; }
    }
}
