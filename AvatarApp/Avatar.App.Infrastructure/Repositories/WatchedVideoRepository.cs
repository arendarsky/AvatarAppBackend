using Avatar.App.Infrastructure.FileStorage.Interfaces;
using Avatar.App.Infrastructure.Models.Casting;

namespace Avatar.App.Infrastructure.Repositories
{
    public class WatchedVideoRepository: EfBaseRepository<WatchedVideoDb>
    {
        public WatchedVideoRepository(AvatarAppContext dbContext, IStorageService storageService) : base(dbContext, storageService)
        {
        }
    }
}
