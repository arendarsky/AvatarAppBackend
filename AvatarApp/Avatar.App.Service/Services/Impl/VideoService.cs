using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Context;
using Avatar.App.Entities.Models;
using Avatar.App.Entities.Settings;
using Microsoft.EntityFrameworkCore;
using Avatar.App.Service.Exceptions;
using Microsoft.Extensions.Options;

namespace Avatar.App.Service.Services.Impl
{
    public class VideoService: IVideoService
    {
        private readonly AvatarAppContext _context;
        private readonly IStorageService _storageService;
        private readonly VideoSettings _videoSettings;

        public VideoService(AvatarAppContext context, IStorageService storageService, IOptions<VideoSettings> videoSOptions)
        {
            _context = context;
            _storageService = storageService;
            _videoSettings = videoSOptions.Value;
        }

        public async Task<Video> UploadVideoAsync(Stream fileStream, Guid userGuid, string fileExtension = null)
        {
            var user = await GetUserAsync(userGuid);

            var newFilename = Path.GetRandomFileName() + fileExtension;
            var video = new Video
            {
                User = user,
                Name = newFilename,
                StartTime = 0,
                EndTime = _videoSettings.FragmentMaxLength
            };

            await _storageService.UploadAsync(fileStream, newFilename);

            await _context.Videos.AddAsync(video);

            await _context.SaveChangesAsync();

            return video;
        }

        public async Task<IEnumerable<Video>> GetUnwatchedVideoListAsync(Guid userGuid, int number)
        {
            var user = await GetUserAsync(userGuid);
            var watchedVideos = _context.WatchedVideos.Where(v => v.UserId == user.Id).Select(c => c.VideoId).ToList();
            var unwatchedVideos = _context.Videos
                .Where(v => v.IsApproved.HasValue && v.IsApproved == true && !watchedVideos.Contains(v.Id))
                .OrderBy(x => Guid.NewGuid())
                .Take(number).ToList();
            return unwatchedVideos;
        }

        public async Task<IEnumerable<Video>> GetUncheckedVideoListAsync(int number)
        {
            var uncheckedVideos = await Task.Run(()=>
            {
                return _context.Videos.Where(v => !v.IsApproved.HasValue).Take(number).ToList();
            });
            return uncheckedVideos;
        }

        public async Task SetVideoFragmentInterval(Guid userGuid, string fileName, double startTime, double endTime)
        {
            if (startTime < 0 || endTime < 0 || endTime <= startTime ||
                endTime - startTime > _videoSettings.FragmentMaxLength) throw new IncorrectFragmentIntervalException();
            var video = await GetVideoAsync(fileName);
            _context.Entry(video).Reference(v => v.User).Load();
            if (video.User.Guid != userGuid) throw new VideoNotFoundException();
            video.StartTime = startTime;
            video.EndTime = endTime;
            await _context.SaveChangesAsync();
        }

        public async Task<Video> GetVideoData(string fileName)
        {
            var video = await GetVideoAsync(fileName);
            return video;
        }

        public async Task<Stream> GetVideoStreamAsync(string fileName)
        {
            return await _storageService.GetFileStreamAsync(fileName);
        }

        public async Task SetLikeAsync(Guid userGuid, string fileName, bool isLike)
        {
            var user = await GetUserAsync(userGuid);
            var video = await GetVideoAsync(fileName);
            var like = await _context.WatchedVideos.FirstOrDefaultAsync(l =>
                l.VideoId == video.Id && l.UserId == user.Id);
            if (like != null) return;
            await _context.WatchedVideos.AddAsync(new WatchedVideo
            {
                User = user,
                Video = video
            });

            if (isLike)
            {
                await _context.LikedVideos.AddAsync(new LikedVideo
                {
                    User = user,
                    Video = video
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task SetApproveStatusAsync(string fileName, bool isApproved)
        {
            var video = await GetVideoAsync(fileName);
            video.IsApproved = isApproved;
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetVideoOwnerAsync(string filename)
        {
            var video = await _context.Videos.Include(v => v.User).FirstOrDefaultAsync(v => v.Name == filename);
            if (video == null) throw new VideoNotFoundException();
            return video.User;
        }

        #region Private Methods

        private async Task<User> GetUserAsync(Guid userGuid)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Guid == userGuid);
            if (user == null) throw new UserNotFoundException();
            return user;
        }

        private async Task<Video> GetVideoAsync(string name)
        {
            var video = await _context.Videos.FirstOrDefaultAsync(v => v.Name == name);
            if (video == null) throw new VideoNotFoundException();
            return video;
        }

        #endregion
    }
}
