using System;
using System.IO;
using Avatar.App.Service.Helpers;
using Avatar.App.Context;
using Avatar.App.Entities.Models;
using System.Threading.Tasks;

namespace Avatar.App.Service.Services.Impl
{
    public class VideoService : IVideoService
    {
        private readonly AvatarAppContext _avatarContext;
        private readonly string _videoStoreDirection = @"C:\VideoDB";
        public VideoService(AvatarAppContext videoContext)
        {
            _avatarContext = videoContext;
        }
        public async Task UploadAsync(Stream uploadedVideoFileStream, Guid userGuid, string fileExtension = null)
        {
            var newFileName = VideoFileHelper.NameGenerator(_videoStoreDirection);
            var fullPathToVideo = _videoStoreDirection + "\\" + newFileName;
            await using (var videoFileStream = new FileStream(@fullPathToVideo, FileMode.Create))
            {
                await uploadedVideoFileStream.CopyToAsync(videoFileStream);
            }


            var video = new Video()
            {
                Name = newFileName
            };
            _avatarContext.Videos.Add(video);
            await _avatarContext.SaveChangesAsync();
        }
        public async Task<VideoStream> GetModeratedVideoAsync()
        {
            var videoStream = new FileStream(_videoStoreDirection + VideoFileHelper.GetRandomPath(_videoStoreDirection), FileMode.Open);
            return new VideoStream
            {
                Stream = videoStream
            };
        }
         public Task<VideoStream> GetUncheckedVideoAsync()
        {
            throw new NotImplementedException();
        }


    }
}
