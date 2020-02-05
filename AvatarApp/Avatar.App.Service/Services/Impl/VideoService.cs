using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Avatar.App.Service.Helpers;
using System.Threading.Tasks;
using Avatar.App.Entities.Models;

namespace Avatar.App.Service.Services.Impl
{
    public class VideoService : IVideoService
    {
        private readonly string _webRootPath;

        public VideoService (string webRootPath)
        {
            _webRootPath = webRootPath;
        }

        public async Task UploadAsync(Stream fileStream, string fileExtension, Guid userGuid)
        {
            var videoFileName = Path.GetRandomFileName();
            await using (var videoFileStream = new FileStream(_webRootPath + videoFileName, FileMode.Create))
            {
                await fileStream.CopyToAsync(videoFileStream);
            }
        }

        public Task<VideoStream> GetUncheckedVideoAsync()
        {
            throw new NotImplementedException();
        }
    }
}
