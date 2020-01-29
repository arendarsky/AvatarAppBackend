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
        private readonly string _webRootPath;

        public VideoService (string webRootPath)
        {
            _webRootPath = webRootPath;
        }

        public async Task<string> Upload(Stream uploadedVideoFileStream)
        {
            var videoFileName = Path.GetRandomFileName();
            await using (var videoFileStream = new FileStream(_webRootPath + videoFileName, FileMode.Create))
            {
                await uploadedVideoFileStream.CopyToAsync(videoFileStream);
            }
            return Path.GetFullPath(videoFileName);
        }

        
    }
}
