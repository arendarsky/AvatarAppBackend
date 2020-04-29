using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Core.Entities;
using Avatar.App.SharedKernel.Settings;
using Avatar.App.Core.Exceptions;
using Avatar.App.Core.Specifications.UserSpecifications;
using Avatar.App.Core.Specifications.VideoSpecifications;
using Avatar.App.SharedKernel;
using Avatar.App.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Avatar.App.Core.Services.Impl
{
    public class VideoService: BaseServiceWithAuthorization, IVideoService
    {
        private readonly IRepository<Video> _videoRepository;
        private readonly IRepository<WatchedVideo> _watchedVideoRepository;
        private readonly IRepository<LikedVideo> _likedVideoRepository;
        private readonly AvatarAppSettings _avatarAppSettings;

        public VideoService(IOptions<AvatarAppSettings> avatarAppOptions, IRepository<Video> videoRepository, IRepository<User> userRepository, IRepository<WatchedVideo> watchedVideoRepository, IRepository<LikedVideo> likedVideoRepository) : base(userRepository)
        {
            _videoRepository = videoRepository;
            _watchedVideoRepository = watchedVideoRepository;
            _likedVideoRepository = likedVideoRepository;
            _avatarAppSettings = avatarAppOptions.Value;
        }

        #region Public Methods

        public async Task<Video> UploadVideoAsync(IFormFile file, Guid userGuid)
        {
            var user = await GetUserAsync(new UserWithLoadedVideosSpecification(userGuid));

            if (!CheckForAvailableVideoSlots(user)) throw new ReachedVideoLimitException();

            var fileName = CreateFileName(file);

            var video = await InsertVideoAsync(user, fileName, file);

            return video;
        }

        public async Task<ICollection<Video>> GetUnwatchedVideosAsync(Guid userGuid, int number)
        {
            var unwatchedVideos = TakeRandomElementsFromVideoList(await GetUnwatchedVideosAsync(userGuid), number);

            return unwatchedVideos.ToList();
        }

        public async Task<ICollection<Video>> GetUncheckedVideosAsync(int number)
        {
            var uncheckedVideos = await GetUncheckedVideosAsync();

            return uncheckedVideos.ToList();
        }

        public async Task SetVideoFragmentInterval(Guid userGuid, string fileName, double startTime, double endTime)
        {
            if (!CheckIntervalCorrectness(startTime, endTime)) throw new IncorrectFragmentIntervalException();

            var video = await GetVideoAsync(new VideoWithUserSpecification(fileName));

            if (!CheckVideoOwner(userGuid, video)) throw new VideoNotFoundException();

            UpdateVideoFragmentInterval(video, startTime, endTime);
        }

        public Stream GetVideoStream(string fileName)
        {
            return GetFile(fileName);
        }

        public async Task SetLikeAsync(Guid userGuid, string fileName, bool isLike)
        {
            var user = await GetUserAsync(new UserSpecification(userGuid));

            var video = await GetVideoAsync(new VideoSpecification(fileName));

            if (!CheckVideoAvailability(video)) throw new VideoNotFoundException();

            if (await CheckLikeExistenceAsync(video, user)) return;

            await InsertWatchedVideoAsync(user, video);

            if (!isLike) return;

            await InsertLikedVideoAsync(user, video);
        }

        public async Task SetApproveStatusAsync(string fileName, bool isApproved)
        {
            var video = await GetVideoAsync(new VideoSpecification(fileName));

            UpdateVideoApproveStatusAsync(video, isApproved);
        }

        public async Task SetActiveAsync(Guid userGuid, string fileName)
        {
            var videos = GetAvailableUserVideos(userGuid).ToList();

            if (!CheckVideoAvailability(videos, fileName)) throw new VideoNotFoundException();

            await UpdateVideosActiveStatusAsync(videos, fileName);
        }

        public async Task RemoveVideoAsync(Guid userGuid, string fileName)
        {
            var video = await GetVideoAsync(new VideoWithUserSpecification(fileName, userGuid));

            if (video == null) throw new VideoNotFoundException();

            RemoveVideo(video);
        }

        public void RemoveAllUnusedFiles()
        {
            var existFiles = GetVideoFilesNames();

            _videoRepository.RemoveFiles(existFiles);
        }

        #endregion

        #region Check Methods

        private bool CheckForAvailableVideoSlots(User user)
        {
            return user.LoadedVideos.Count() < _avatarAppSettings.MaxVideoNumber;
        }

        private bool CheckIntervalCorrectness(double startTime, double endTime)
        {
            return startTime >= 0 && endTime > 0 && endTime > startTime &&
                   endTime - startTime <= _avatarAppSettings.ShortVideoMaxLength;
        }

        private static bool CheckVideoOwner(Guid userGuid, Video video)
        {
            return video.User.Guid == userGuid;
        }

        private static bool CheckVideoAvailability(Video video)
        {
            return video.IsApproved.HasValue && video.IsApproved == true;
        }

        private async Task<bool> CheckLikeExistenceAsync(BaseEntity video, BaseEntity user)
        {
            var like = await _watchedVideoRepository.GetAsync(l =>
                l.VideoId == video.Id && l.UserId == user.Id);
            return like != null;
        }

        private static bool CheckVideoAvailability(IEnumerable<Video> availableVideos, string fileName)
        {
            return availableVideos.FirstOrDefault(v => v.Name == fileName) != null;
        }

        #endregion

        #region Get Methods

        private async Task<Video> GetVideoAsync(ISpecification<Video> specification)
        {
            var video = await _videoRepository.GetAsync(specification);

            if (video == null) throw new VideoNotFoundException();

            return video;
        }

        private async Task<IEnumerable<Video>> GetUnwatchedVideosAsync(Guid userGuid)
        {
            var user = await GetUserAsync(new UserWithWatchedVideosSpecification(userGuid));

            var watchedVideos = user.WatchedVideos.Select(watchedVideo => watchedVideo.VideoId);

            return _videoRepository.List(new VideoWithUserSpecification(watchedVideos));
        }

        private async Task<IEnumerable<Video>> GetUncheckedVideosAsync()
        {
            var uncheckedVideos = await Task.Run(() => _videoRepository.List(new VideoWithUserSpecification()));

            return uncheckedVideos;
        }

        private IEnumerable<Video> GetAvailableUserVideos(Guid userGuid)
        {
            return _videoRepository.List(new VideoSpecification(userGuid));
        }

        private static IEnumerable<Video> TakeRandomElementsFromVideoList(IEnumerable<Video> videoList, int number)
        {
            return videoList.OrderBy(c => Guid.NewGuid()).Take(number);
        }

        private Stream GetFile(string fileName)
        {
            return _videoRepository.GetFile(fileName);
        }

        private ICollection<string> GetVideoFilesNames()
        {
            return _videoRepository.List().Select(video => video.Name).ToList();
        }


        #endregion

        #region Create Methods

        private static string CreateFileName(IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName);
            return Path.GetRandomFileName() + fileExtension;
        }

        #endregion

        #region Insert Methods

        private async Task<Video> InsertVideoAsync(User user, string fileName, IFormFile file)
        {
            var video = new Video
            {
                User = user,
                Name = fileName,
                StartTime = 0,
                EndTime = _avatarAppSettings.ShortVideoMaxLength,
                IsApproved = true,
                IsActive = !user.LoadedVideos.Any()
            };

            await _videoRepository.InsertFileAsync(file, fileName);

            await _videoRepository.InsertAsync(video);

            return video;
        }

        private async Task InsertWatchedVideoAsync(User user, Video video)
        {
            await _watchedVideoRepository.InsertAsync(new WatchedVideo
            {
                User = user,
                Video = video
            });

        }

        private async Task InsertLikedVideoAsync(User user, Video video)
        {
            await _likedVideoRepository.InsertAsync(new LikedVideo
            {
                User = user,
                Video = video,
                Date = DateTime.Now
            });
        }

        #endregion

        #region Update Methods

        private void UpdateVideoFragmentInterval(Video video, double startTime, double endTime)
        {
            video.StartTime = startTime;
            video.EndTime = endTime;

            _videoRepository.Update(video);
        }

        private void UpdateVideoApproveStatusAsync(Video video, bool isApproved)
        {
            video.IsApproved = isApproved;

            _videoRepository.Update(video);
        }

        private async Task UpdateVideosActiveStatusAsync(ICollection<Video> videos, string fileName)
        {
            await Task.Run(() =>
            {
                foreach (var video in videos)
                {
                    video.IsActive = video.Name == fileName;
                }

                _videoRepository.UpdateRange(videos);
            });
        }

        #endregion

        #region Remove Methods

        private void RemoveVideo(Video video)
        {
            _watchedVideoRepository.Delete(v => v.VideoId == video.Id);
            _likedVideoRepository.Delete(v => v.VideoId == video.Id);

            _videoRepository.Delete(video);
        }

        #endregion
    }
}
