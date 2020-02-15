﻿using System.IO;
using System.Threading.Tasks;

namespace Avatar.App.Service.Services
{
    public interface IStorageService
    {
        Task UploadAsync(Stream fileStream, string fileName);
        Task<Stream> GetFileStreamAsync(string fileName);
    }
}
