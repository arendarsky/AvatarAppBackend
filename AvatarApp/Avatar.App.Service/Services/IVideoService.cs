using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Avatar.App.Service.Services
{
    public interface IVideoService
    {
        Task<string> UploadAsync(Stream fileStream, string fileExtension);
        Task<Stream> GetUncheckedVideoAsync();
    }
}
