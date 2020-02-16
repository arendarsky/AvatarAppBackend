using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Context;
using Avatar.App.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Avatar.App.Service.Services.Impl
{
    public class VideoService: IVideoService
    {
        private readonly AvatarAppContext _context;
        private readonly IStorageService _storageService;

        public VideoService(AvatarAppContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task UploadVideoAsync(Stream fileStream, Guid userGuid, string fileExtension = null)
        {
            var user = await GetUserAsync(userGuid);

            var newFilename = Path.GetRandomFileName() + fileExtension;
            var video = new Video
            {
                User = user,
                Name = newFilename
            };

            await _storageService.UploadAsync(fileStream, newFilename);

            await _context.Videos.AddAsync(video);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Video>> GetUnwatchedVideoListAsync(Guid userGuid, int number)
        {
            var user = await GetUserAsync(userGuid);
            _context.Entry(user).Collection(u => u.WatchedVideos).Load();
            var watchedVideos = user.WatchedVideos.Select(v => v.Video.Id);
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

        public async Task<Stream> GetVideoStreamAsync(string fileName)
        {
            return await _storageService.GetFileStreamAsync(fileName);
        }

        public async Task SetLikeAsync(Guid userGuid, string fileName, bool isLike)
        {
            var user = await GetUserAsync(userGuid);
            var video = await GetVideoAsync(fileName);
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

        #region Private Methods

        private async Task<User> GetUserAsync(Guid userGuid)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Guid == userGuid);
            if (user == null) throw new NullReferenceException();
            return user;
        }

        private async Task<Video> GetVideoAsync(string name)
        {
            var video = await _context.Videos.FirstOrDefaultAsync(v => v.Name == name);
            if (video == null) throw new NullReferenceException();
            return video;
        }

        #endregion
    }
}
