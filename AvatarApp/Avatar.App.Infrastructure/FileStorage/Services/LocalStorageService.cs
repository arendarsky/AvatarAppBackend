using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Infrastructure.FileStorage.Interfaces;
using Avatar.App.SharedKernel.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Avatar.App.Infrastructure.FileStorage.Services
{
    public class LocalStorageService : IStorageService
    {
        private readonly EnvironmentConfig _environmentConfig;

        public LocalStorageService(IOptions<EnvironmentConfig> environmentConfig)
        {
            _environmentConfig = environmentConfig.Value;
        }

        #region Public Methods

        public async Task UploadAsync(IFormFile file, string fileName, string storagePrefix)
        {
            var fullPath = CreateFilePath(storagePrefix, fileName);

            await SaveFileAsync(fullPath, file);
        }


        public Stream GetFileStream(string fileName, string storagePrefix)
        {
            return CreateFileStreamAsync(fileName, storagePrefix);
        }

        public void RemoveUnusedFiles(ICollection<string> existFiles, string storagePrefix)
        {
            var directory = new DirectoryInfo(_environmentConfig.STORAGE_PATH + storagePrefix);

            RemoveUnusedFiles(existFiles, directory);
        }

        #endregion

        #region Private Methods

        private string CreateFilePath(string storagePrefix, string fileName)
        {
            return _environmentConfig.STORAGE_PATH + storagePrefix + fileName;
        }

        private static async Task SaveFileAsync(string fullPath, IFormFile file)
        {
            await using var outputFileStream = File.Create(fullPath);
            await file.CopyToAsync(outputFileStream);
        }

        private Stream CreateFileStreamAsync(string fileName, string storagePrefix)
        {
            var fullVideoPath = CreateFilePath(storagePrefix, fileName);

            var fileStream = new FileStream(fullVideoPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096,
                FileOptions.Asynchronous);

            return fileStream;
        }

        private static void RemoveUnusedFiles(ICollection<string> existFiles, DirectoryInfo directory)
        {
            foreach (var file in directory.GetFiles())
            {
                if (!existFiles.Contains(file.Name)) File.Delete(file.FullName);
            }
        }

        #endregion

    }
}
