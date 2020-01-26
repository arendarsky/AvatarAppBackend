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
        private readonly string videoStoreDirection = "";

        public async Task<string> Upload(Stream upploadedVideoFileStream)
        {
            string nameOfVideo = videoStoreDirection + "\\" + VideoFileNameGeneratorHelper.NameGenerator(videoStoreDirection);
            using (var videoFileStream = new FileStream(nameOfVideo, FileMode.Create))
            {
                await upploadedVideoFileStream.CopyToAsync(videoFileStream);
            }
            return Path.GetFullPath(nameOfVideo);
        }

        
    }
}
