using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avatar.App.Entities.Models;

namespace Avatar.App.Service.Services
{
    public interface IVideoService
    {
        Task<Video> UploadVideoAsync(Stream fileStream, Guid userGuid, string fileExtension = null);
        Task<ICollection<Video>> GetUnwatchedVideoListAsync(Guid userGuid, int number);
        Task<ICollection<Video>> GetUncheckedVideoListAsync(int number);
        Task SetVideoFragmentInterval(Guid userGuid, string fileName, double startTime, double endTime);

        Task<Stream> GetVideoStreamAsync(string fileName);

        Task SetLikeAsync(Guid userGuid, string fileName, bool isLike);

        Task SetApproveStatusAsync(string fileName, bool isApproved);

        Task<User> GetVideoOwnerAsync(string filename);

    }
}
