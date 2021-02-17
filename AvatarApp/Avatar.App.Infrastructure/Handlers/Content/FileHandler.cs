using Avatar.App.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Avatar.App.Infrastructure.Handlers.Content
{
    internal abstract class FileHandler
    {
        protected readonly EnvironmentConfig EnvironmentConfig;

        protected FileHandler(IOptions<EnvironmentConfig> environmentConfig)
        {
            EnvironmentConfig = environmentConfig.Value;
        }

        protected string GetFilePath(string fileName, string storagePrefix)
        {
            return EnvironmentConfig.STORAGE_PATH + storagePrefix + fileName;
        }
    }
}
