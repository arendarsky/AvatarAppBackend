using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avatar.App.Entities.Models;

namespace Avatar.App.Service.Services
{
    public interface IVideoService
    {
        Task UploadVideoAsync(Stream fileStream, Guid userGuid, string fileExtension = null);
        Task<IEnumerable<Video>> GetUnwatchedVideoListAsync(Guid userGuid, int number);
        Task<IEnumerable<Video>> GetUncheckedVideoListAsync(int number);

        Task<Stream> GetVideoStreamAsync(string fileName);

        Task SetLikeAsync(Guid userGuid, string fileName, bool isLike);

        Task SetApproveStatusAsync(string fileName, bool isApproved);

    }
}
