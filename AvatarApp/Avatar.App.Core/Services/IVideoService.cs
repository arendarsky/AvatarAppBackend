using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avatar.App.Core.Entities;

namespace Avatar.App.Core.Services
{
    public interface IVideoService
    {
        Task<Video> UploadVideoAsync(Stream fileStream, Guid userGuid, string fileExtension = null);
        Task<ICollection<Video>> GetUnwatchedVideosAsync(Guid userGuid, int number);
        Task<ICollection<Video>> GetUncheckedVideosAsync(int number);
        Task SetVideoFragmentInterval(Guid userGuid, string fileName, double startTime, double endTime);
        Stream GetVideoStream(string fileName);
        Task SetLikeAsync(Guid userGuid, string fileName, bool isLike);
        Task SetApproveStatusAsync(string fileName, bool isApproved);
        Task SetActiveAsync(Guid userGuid, string fileName);
        Task RemoveVideoAsync(Guid userGuid, string fileName);

    }
}
