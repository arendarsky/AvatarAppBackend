using Avatar.App.Infrastructure.FileStorage.Interfaces;
using Avatar.App.Infrastructure.Models.Casting;

namespace Avatar.App.Infrastructure.Repositories
{
    public class LikedVideoRepository: EfBaseRepository<LikedVideoDb>
    {
        public LikedVideoRepository(AvatarAppContext dbContext, IStorageService storageService) : base(dbContext, storageService)
        {
        }
    }
}
