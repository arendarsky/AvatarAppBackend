using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Avatar.App.Service.Helpers;
using Avatar.App.Context;
using Avatar.App.Entities.Models;
using System.Threading.Tasks;

namespace Avatar.App.Service.Services.Impl
{
    public class VideoService : IVideoService
    {
        private readonly AvatarContext _avatarContext;
        private readonly string _videoStoreDirection = @"C:\VideoDB";
        public VideoService(AvatarContext videoContext)
        {
            _avatarContext = videoContext;
        }
        public async Task<string> Upload(Stream uploadedVideoFileStream)
        {
            var fullPathToVideo = _videoStoreDirection + "\\" + VideoFileHelper.NameGenerator(_videoStoreDirection);
            await using (var videoFileStream = new FileStream(@fullPathToVideo, FileMode.Create))
            {
                await uploadedVideoFileStream.CopyToAsync(videoFileStream);
            }


            Video video = new Video()
            {
                fullPath = fullPathToVideo
            };
            _avatarContext.Videos.Add(video);
            await _avatarContext.SaveChangesAsync();
            return Path.GetFullPath(fullPathToVideo);
        }
        public async Task<Stream> GetRandomVideoStream()
        {
            Stream VideoStream = new FileStream(_videoStoreDirection + VideoFileHelper.GetRandomPath(_videoStoreDirection), FileMode.Open);
            return VideoStream;
        }

    }
}
