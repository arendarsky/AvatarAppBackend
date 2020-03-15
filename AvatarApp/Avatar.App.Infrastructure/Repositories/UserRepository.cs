using Avatar.App.Core.Entities;
using Avatar.App.Infrastructure.FileStorage.Interfaces;
using Avatar.App.SharedKernel.Settings;
using Microsoft.Extensions.Options;

namespace Avatar.App.Infrastructure.Repositories
{
    public class UserRepository: EfBaseRepository<User>
    {

        public UserRepository(AvatarAppContext dbContext, IStorageService storageService, IOptions<AvatarAppSettings> avatarAppOptions) : base(dbContext, storageService)
        {
            StoragePrefix = avatarAppOptions.Value.ImageStoragePrefix;
        }

    }
}
