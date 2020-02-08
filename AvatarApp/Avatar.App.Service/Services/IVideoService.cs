using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Avatar.App.Entities.Models;

namespace Avatar.App.Service.Services
{
    public interface IVideoService
    {
        Task UploadVideoAsync(Stream fileStream, Guid userGuid, string fileExtension = null);
        Task<IEnumerable<Video>> GetUnwatchedVideoListAsync(Guid userGuid, int number);
        IEnumerable<Video> GetUncheckedVideoList(int number);

        Task<Stream> GetVideoStreamAsync(string fileName);
    }
}
