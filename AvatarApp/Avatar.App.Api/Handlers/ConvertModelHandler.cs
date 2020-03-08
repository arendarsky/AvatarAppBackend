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
                let loadedVideoModels = video.User.LoadedVideos.Select(v => new VideoModel
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
                    Videos = loadedVideoModels,
                    Guid = video.User.Guid.ToString()
                }).ToList();
        }

        public static ICollection<RatingItemModel> RatingItemsToRatingItemModels(ICollection<RatingItem> ratingItems)
        {
            return (from ratingItem in ratingItems
                let videoModels = ratingItem.User.LoadedVideos.Select(v => new VideoModel
                {
                    Name = v.Name,
                    StartTime = v.StartTime,
                    EndTime = v.EndTime,
                    IsActive = v.IsActive
                }).ToList()
                select new RatingItemModel
                {
                    LikesNumber = ratingItem.LikesNumber,
                    User = new UserModel
                    {
                        Name = ratingItem.User.Name,
                        Description = ratingItem.User.Description,
                        Guid = ratingItem.User.Guid.ToString(),
                        Videos = videoModels
                    }
                }).ToList();
        }

        public static ICollection<LikedVideoModel> LikedVideosToLikedVideoModels(ICollection<LikedVideo> likedVideos)
        {
            return (from likedVideo in likedVideos
                    let videoModels = likedVideo.User.LoadedVideos.Select(v => new VideoModel
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
                        Description = likedVideo.User.Description,
                        Guid = likedVideo.User.Guid.ToString(),
                        Videos = videoModels
                    }
                }).ToList();
        }
    }
}
