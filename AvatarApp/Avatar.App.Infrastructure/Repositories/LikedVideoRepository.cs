using Avatar.App.Core.Entities;
using Avatar.App.Infrastructure.FileStorage.Interfaces;

namespace Avatar.App.Infrastructure.Repositories
{
    public class LikedVideoRepository: EfBaseRepository<LikedVideo>
    {
        public LikedVideoRepository(AvatarAppContext dbContext, IStorageService storageService) : base(dbContext, storageService)
        {
        }
    }
}
