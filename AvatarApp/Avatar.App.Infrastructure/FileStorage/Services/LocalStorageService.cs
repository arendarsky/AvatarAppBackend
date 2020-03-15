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


        public async Task<Stream> GetFileStreamAsync(string fileName, string storagePrefix)
        {
            return await CreateFileStreamAsync(fileName, storagePrefix);
        }

        #endregion

        #region Private Methods

        private string CreateFilePath(string storagePrefix, string fileName)
        {
            return _environmentConfig.STORAGE_PATH + storagePrefix + fileName;
        }

        private static async Task SaveFileAsync(string fullPath, Stream inputFileStream)
        {
            await using var outputFileStream = new FileStream(fullPath, FileMode.Create);
            await inputFileStream.CopyToAsync(outputFileStream);
        }

        private async Task<Stream> CreateFileStreamAsync(string fileName, string storagePrefix)
        {
            var fileStream = await Task.Run(() =>
            {
                var fullVideoPath = CreateFilePath(storagePrefix, fileName);
                return new FileStream(fullVideoPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096,
                    FileOptions.Asynchronous);
            });
            return fileStream;
        }

        #endregion

    }
}
