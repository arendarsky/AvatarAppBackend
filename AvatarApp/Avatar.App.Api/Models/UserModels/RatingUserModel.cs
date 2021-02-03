﻿using System.Linq;
using Avatar.App.Infrastructure.Models.Casting;

namespace Avatar.App.Api.Models.UserModels
{
    public class RatingUserModel: BaseUserModel
    {
        public RatingUserModel(UserDb user, int likesNumber) : base(user)
        {
            LikesNumber = likesNumber;

            var video = user.LoadedVideos?.FirstOrDefault(c =>
                            c.IsApproved.HasValue && c.IsApproved == true && c.IsActive) ??
                        user.LoadedVideos?.FirstOrDefault(c =>
                            c.IsApproved.HasValue && c.IsApproved == true);

            if (video == null) return;

            Video = new VideoModel(video);
        }

        public int LikesNumber { get; set; }
        public VideoModel Video { get; set; }
    }
}
