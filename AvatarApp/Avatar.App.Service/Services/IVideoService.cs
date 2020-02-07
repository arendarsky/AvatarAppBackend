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
        Task UploadAsync(Stream fileStream, Guid userGuid, string fileExtension = null);

        Task<VideoStream> GetModeratedVideoAsync();

        Task<VideoStream> GetUncheckedVideoAsync();
    }
}
