using System.Collections.Generic;
using System.Linq;
using Avatar.App.Api.Models.UserModels;
using Avatar.App.Core.Entities;
using Avatar.App.Core.Models;

namespace Avatar.App.Api.Handlers
{
    public static class ConvertModelHandler
    {
        public static ICollection<VideoUserModel> VideosToVideoUserModels(ICollection<Video> videos)
        {
            return videos.Select(v => new VideoUserModel(v)).ToList();
        }

        public static ICollection<ModerationUserModel> VideosToModerationUserModels(ICollection<Video> videos)
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

        public static ICollection<NotificationUserModel> LikedVideosToNotificationUserModel(ICollection<LikedVideo> likedVideos)
        {
            return likedVideos.Select(l => new NotificationUserModel(l)).ToList();
        }

        public static PublicProfileUserModel UserToPublicUserProfile(User user)
        {
            return new PublicProfileUserModel(user);
        }
    }
}
