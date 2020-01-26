using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Avatar.App.Service.Helpers;
using System.Threading.Tasks;

namespace Avatar.App.Service.Services.Impl
{
    public class VideoService : IVideoService
    {
        private readonly string _videoStoreDirection = "";

        public async Task<string> Upload(Stream uploadedVideoFileStream)
        {
            var nameOfVideo = _videoStoreDirection + "\\" + VideoFileHelper.NameGenerator(_videoStoreDirection);
            await using (var videoFileStream = new FileStream(nameOfVideo, FileMode.Create))
            {
                await uploadedVideoFileStream.CopyToAsync(videoFileStream);
            }
            return Path.GetFullPath(nameOfVideo);
        }

        
    }
}
