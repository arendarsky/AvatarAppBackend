using Avatar.App.Infrastructure.FileStorage.Interfaces;
using Avatar.App.Infrastructure.Models.Casting;
using Avatar.App.SharedKernel.Interfaces;
using Avatar.App.SharedKernel.Settings;
using Microsoft.Extensions.Options;

namespace Avatar.App.Infrastructure.Repositories
{
    public class VideoRepository: EfBaseRepository<VideoDb>, IRepository<VideoDb>
    {
        public VideoRepository(AvatarAppContext dbContext, IStorageService storageService, IOptions<AvatarAppSettings> avatarAppOptions) : base(dbContext, storageService)
        {
            StoragePrefix = avatarAppOptions.Value.VideoStoragePrefix;
        }
    }
}
