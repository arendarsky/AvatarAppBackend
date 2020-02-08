using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Avatar.App.Service.Services
{
    public interface IStorageService
    {
        Task UploadAsync(Stream fileStream, string fileName, string fileExtension = null);
        Task<Stream> GetFileStreamAsync(string fileName);
    }
}
