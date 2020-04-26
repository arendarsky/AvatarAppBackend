using System.IO;
using System.Threading.Tasks;
using Avatar.App.Infrastructure.FileStorage.Interfaces;
using Avatar.App.SharedKernel.Settings;
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

        public async Task UploadAsync(Stream fileStream, string fileName, string storagePrefix)
        {
            var fullPath = CreateFilePath(storagePrefix, fileName);

            await SaveFileAsync(fullPath, fileStream);
        }


        public Stream GetFileStream(string fileName, string storagePrefix)
        {
            return CreateFileStreamAsync(fileName, storagePrefix);
        }

        #endregion

        #region Private Methods

        private string CreateFilePath(string storagePrefix, string fileName)
        {
            return _environmentConfig.STORAGE_PATH + storagePrefix + fileName;
        }

        private static async Task SaveFileAsync(string fullPath, Stream inputFileStream)
        {
            await using var outputFileStream =
                new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None);
            await inputFileStream.CopyToAsync(outputFileStream);
        }

        private Stream CreateFileStreamAsync(string fileName, string storagePrefix)
        {
            var fullVideoPath = CreateFilePath(storagePrefix, fileName);

            var fileStream = new FileStream(fullVideoPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096,
                FileOptions.Asynchronous);

            return fileStream;
        }

        #endregion

    }
}
