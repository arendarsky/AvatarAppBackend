using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

            var newFilename = Path.GetRandomFileName();
            var video = new Video
            {
                User = user,
                Name = newFilename,
                Extension = fileExtension
            };

            await _storageService.UploadAsync(fileStream, newFilename, fileExtension);

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

        public IEnumerable<Video> GetUncheckedVideoList(int number)
        {
            var uncheckedVideos = _context.Videos.Where(v => !v.IsApproved.HasValue).Take(number).ToList();
            return uncheckedVideos;
        }

        public async Task<Stream> GetVideoStreamAsync(string fileName)
        {
            return await _storageService.GetFileStreamAsync(fileName);
        }

        #region Private Methods

        private async Task<User> GetUserAsync(Guid userGuid)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Guid == userGuid);
            if (user == null) throw new NullReferenceException();
            return user;
        }

        #endregion
    }
}
