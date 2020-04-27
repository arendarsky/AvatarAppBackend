using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Avatar.App.Infrastructure.FileStorage.Interfaces
{
    public interface IStorageService
    {
        /// <summary>
        /// Save file from stream to local disk
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="fileName"></param>
        /// <param name="storagePrefix"></param>
        /// <returns></returns>
        Task UploadAsync(Stream fileStream, string fileName, string storagePrefix);

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
        void RemoveUnusedFiles(List<string> existFiles, string storagePrefix);

    }
}
