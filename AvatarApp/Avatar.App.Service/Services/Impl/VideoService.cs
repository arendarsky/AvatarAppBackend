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

        public async Task<string> Upload(byte[] videoBytes)
        {
            string nameOfVideo = videoStoreDirection + "\\" + VideoFileNameGeneratorHelper.NameGenerator(videoStoreDirection);
            var videoFile = new StreamWriter(nameOfVideo);
            videoFile.Write(Encoding.Unicode.GetString(videoBytes));
            videoFile.Close();
            return Path.GetFullPath(nameOfVideo);
        }

        
    }
}
