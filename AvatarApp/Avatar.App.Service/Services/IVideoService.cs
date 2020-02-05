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
        Task UploadAsync(Stream fileStream, string fileExtension, Guid userGuid);
        Task<VideoStream> GetUncheckedVideoAsync();
    }
}
