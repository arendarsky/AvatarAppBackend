using System;
using System.Collections.Generic;
using System.Text;
using Avatar.App.Core.Entities;
using Avatar.App.Core.Models;
using Avatar.App.SharedKernel.Interfaces;

namespace Avatar.App.Core.Services.Impl
{
    public class AnalyticsService: IAnalyticsService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Video> _videoRepository;
        private readonly IRepository<LikedVideo> _likedVideoRepository;
        private readonly IRepository<WatchedVideo> _watchedVideoRepository;

        public AnalyticsService(IRepository<User> userRepository, IRepository<Video> videoRepository,
            IRepository<LikedVideo> likedVideoRepository, IRepository<WatchedVideo> watchedVideoRepository)
        {
            _userRepository = userRepository;
            _videoRepository = videoRepository;
            _likedVideoRepository = likedVideoRepository;
            _watchedVideoRepository = watchedVideoRepository;
        }

        public GeneralStatisticsDto GetGeneralStatistics()
        {
            var totalUsers = _userRepository.Count(u => true);
            var totalVideos = _videoRepository.Count(v => true);
            var totalActiveVideos = _videoRepository.Count(video => video.IsActive == true);
            var totalViews = _watchedVideoRepository.Count(w => true);
            var totalLikes = _likedVideoRepository.Count(l => true);

            return new GeneralStatisticsDto
            {
                TotalUsers = totalUsers,
                TotalVideos = totalVideos,
                TotalActiveVideos = totalActiveVideos,
                TotalViews = totalViews,
                TotalLikes = totalLikes
            };
        }
    }
}
