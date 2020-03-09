using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Api.Models;
using Avatar.App.Entities.Models;
using Avatar.App.Service.Models;

namespace Avatar.App.Api.Handlers
{
    public static class ConvertModelHandler
    {
        public static ICollection<UserModel> VideosToUserModels(ICollection<Video> videos)
        {
            return (from video in videos
                let loadedVideoModels = video.User.LoadedVideos
                    .Where(v => v.IsApproved.HasValue && v.IsApproved == true).Select(v => new VideoModel
                    {
                        Name = v.Name,
                        StartTime = v.StartTime,
                        EndTime = v.EndTime,
                        IsActive = v.IsActive
                    }).ToList()
                select new UserModel
                {
                    Name = video.User.Name,
                    Description = video.User.Description,
                    ProfilePhoto = video.User.ProfilePhoto,
                    Videos = loadedVideoModels,
                    Guid = video.User.Guid.ToString()
                }).ToList();
        }

        public static ICollection<UserProfileModel> UserProfilesToUserProfileModels(ICollection<UserProfile> userProfiles)
        {
            return (from userProfile in userProfiles

                select UserProfileToUserProfileModel(userProfile)).ToList();
        }

        public static UserProfileModel UserProfileToUserProfileModel(UserProfile userProfile)
        {
            var videoModels = userProfile.User.LoadedVideos.Where(v => v.IsApproved.HasValue && v.IsApproved == true)
                .Select(v => new VideoModel
                {
                    Name = v.Name,
                    StartTime = v.StartTime,
                    EndTime = v.EndTime,
                    IsActive = v.IsActive
                }).ToList();
            return new UserProfileModel
            {
                LikesNumber = userProfile.LikesNumber,
                User = new UserModel
                {
                    Name = userProfile.User.Name,
                    Description = userProfile.User.Description,
                    ProfilePhoto = userProfile.User.ProfilePhoto,
                    Guid = userProfile.User.Guid.ToString(),
                    Videos = videoModels
                }
            };
        }

        public static ICollection<LikedVideoModel> LikedVideosToLikedVideoModels(ICollection<LikedVideo> likedVideos)
        {
            return (from likedVideo in likedVideos
                let videoModels = likedVideo.User.LoadedVideos.Where(v => v.IsApproved.HasValue && v.IsApproved == true)
                    .Select(v => new VideoModel
                    {
                        Name = v.Name,
                        StartTime = v.StartTime,
                        EndTime = v.EndTime,
                        IsActive = v.IsActive
                    }).ToList()
                select new LikedVideoModel()
                {
                    Date = likedVideo.Date,
                    User = new UserModel
                    {
                        Name = likedVideo.User.Name,
                        ProfilePhoto = likedVideo.User.ProfilePhoto,
                        Description = likedVideo.User.Description,
                        Guid = likedVideo.User.Guid.ToString(),
                        Videos = videoModels
                    }
                }).ToList();
        }
    }
}
