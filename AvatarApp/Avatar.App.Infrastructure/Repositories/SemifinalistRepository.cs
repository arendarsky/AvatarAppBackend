using Avatar.App.Infrastructure.FileStorage.Interfaces;
using Avatar.App.Infrastructure.Models.Semifinal;

namespace Avatar.App.Infrastructure.Repositories
{
    public class SemifinalistRepository: EfBaseRepository<SemifinalistDb>
    {
        public SemifinalistRepository(AvatarAppContext dbContext, IStorageService storageService) : base(dbContext, storageService)
        {
        }
    }
}
