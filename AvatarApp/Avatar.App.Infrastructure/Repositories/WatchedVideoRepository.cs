using Avatar.App.Core.Entities;
using Avatar.App.Infrastructure.FileStorage.Interfaces;

namespace Avatar.App.Infrastructure.Repositories
{
    public class WatchedVideoRepository: EfBaseRepository<WatchedVideo>
    {
        public WatchedVideoRepository(AvatarAppContext dbContext, IStorageService storageService) : base(dbContext, storageService)
        {
        }
    }
}
