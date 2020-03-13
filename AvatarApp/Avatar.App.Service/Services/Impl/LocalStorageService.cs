using System.IO;
using System.Threading.Tasks;
using Avatar.App.Entities.Settings;
using Microsoft.Extensions.Options;

namespace Avatar.App.Service.Services.Impl
{
    public class LocalStorageService : IStorageService
    {
        private readonly AvatarAppSettings _avatarAppSettings;
        private readonly EnvironmentConfig _environmentConfig;

        public LocalStorageService(IOptions<EnvironmentConfig> environmentConfig, IOptions<AvatarAppSettings> avatarAppOptions)
        {
            _avatarAppSettings = avatarAppOptions.Value;
            _environmentConfig = environmentConfig.Value;
        }

        public async Task UploadAsync(Stream fileStream, string fileName, string storagePrefix)
        {
            var fullPath = CreateFilePath(storagePrefix, fileName);

            await SaveFileAsync(fullPath, fileStream);
        }


        public async Task<Stream> GetFileStreamAsync(string fileName, string storagePrefix)
        {
            return await CreateFileStreamAsync(fileName, storagePrefix);
        }

        #region Private Methods

        private string CreateFilePath(string storagePrefix, string fileName)
        {
            return _environmentConfig.STORAGE_PATH + storagePrefix + fileName;
        }

        private async Task SaveFileAsync(string fullPath, Stream inputFileStream)
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
