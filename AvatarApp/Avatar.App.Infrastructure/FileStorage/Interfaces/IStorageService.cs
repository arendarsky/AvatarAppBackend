using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Avatar.App.Infrastructure.FileStorage.Interfaces
{
    public interface IStorageService
    {
        /// <summary>
        /// Save file from stream to local disk
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileName"></param>
        /// <param name="storagePrefix"></param>
        /// <returns></returns>
        Task UploadAsync(IFormFile file, string fileName, string storagePrefix);

        /// <summary>
        /// Get stream from file on local disk
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="storagePrefix"></param>
        /// <returns></returns>
        Stream GetFileStream(string fileName, string storagePrefix);

        /// <summary>
        /// Get stream from file on local disk
        /// </summary>
        /// <param name="existFiles"></param>
        /// <param name="storagePrefix"></param>
        /// <returns></returns>
        void RemoveUnusedFiles(ICollection<string> existFiles, string storagePrefix);

    }
}
