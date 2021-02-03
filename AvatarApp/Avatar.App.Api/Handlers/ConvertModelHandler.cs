using System.Collections.Generic;
using System.Linq;
using Avatar.App.Api.Models.UserModels;
using Avatar.App.Core.Models;
using Avatar.App.Infrastructure.Models.Casting;

namespace Avatar.App.Api.Handlers
{
    public static class ConvertModelHandler
    {
        public static ICollection<VideoUserModel> VideosToVideoUserModels(ICollection<VideoDb> videos)
        {
            return videos.Select(v => new VideoUserModel(v)).ToList();
        }

        public static ICollection<ModerationUserModel> VideosToModerationUserModels(ICollection<VideoDb> videos)
        {
            return videos.Select(v => new ModerationUserModel(v)).ToList();
        }

        public static ICollection<RatingUserModel> UserProfilesToRatingUserModels(ICollection<UserProfile> userProfiles)
        {
            return userProfiles.Select(u => new RatingUserModel(u.User, u.LikesNumber)).ToList();
        }

        public static PrivateProfileUserModel UserProfileToPrivateProfileUserModel(UserProfile userProfile)
        {
            return new PrivateProfileUserModel(userProfile.User, userProfile.LikesNumber);
        }

        public static ICollection<NotificationUserModel> LikedVideosToNotificationUserModel(ICollection<LikedVideoDb> likedVideos)
        {
            return likedVideos.Select(l => new NotificationUserModel(l)).ToList();
        }

        public static PublicProfileUserModel UserToPublicUserProfile(UserDb user)
        {
            return new PublicProfileUserModel(user);
        }

        public static ICollection<SemifinalistUserModel> UserProfilesToSemifinalistUserModels(ICollection<UserProfile> usersProfiles)
        {
            return usersProfiles.Select(user => new SemifinalistUserModel(user)).ToList();
        }
    }
}
