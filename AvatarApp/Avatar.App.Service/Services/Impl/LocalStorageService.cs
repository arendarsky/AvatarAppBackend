using System.IO;
using System.Threading.Tasks;
using Avatar.App.Entities.Settings;
using Microsoft.Extensions.Options;

namespace Avatar.App.Service.Services.Impl
{
    public class LocalStorageService : IStorageService
    {
        private readonly EnvironmentConfig _environmentConfig;

        public LocalStorageService(IOptions<EnvironmentConfig> environmentConfig)
        {
            _environmentConfig = environmentConfig.Value;
        }

        public async Task UploadAsync(Stream fileStream, string fileName, string storagePrefix)
        {
            var fullVideoPath = _environmentConfig.STORAGE_PATH + storagePrefix + fileName;
            await using var videoFileStream = new FileStream(fullVideoPath, FileMode.Create);
            await fileStream.CopyToAsync(videoFileStream);
        }

        public async Task<Stream> GetFileStreamAsync(string fileName, string storagePrefix)
        {
            var fileStream = await Task.Run(() =>
            {
                var fullVideoPath = _environmentConfig.STORAGE_PATH + storagePrefix + fileName;
                return new FileStream(fullVideoPath, FileMode.Open, FileAccess.Read);
            });
            return fileStream;
        }

    }
}
