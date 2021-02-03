using Avatar.App.Infrastructure.FileStorage.Interfaces;
using Avatar.App.Infrastructure.Models.Casting;
using Avatar.App.SharedKernel.Settings;
using Microsoft.Extensions.Options;

namespace Avatar.App.Infrastructure.Repositories
{
    public class UserRepository: EfBaseRepository<UserDb>
    {

        public UserRepository(AvatarAppContext dbContext, IStorageService storageService, IOptions<AvatarAppSettings> avatarAppOptions) : base(dbContext, storageService)
        {
            StoragePrefix = avatarAppOptions.Value.ImageStoragePrefix;
        }

    }
}
