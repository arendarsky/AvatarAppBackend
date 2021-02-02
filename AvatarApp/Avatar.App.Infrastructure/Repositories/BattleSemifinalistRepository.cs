using Avatar.App.Core.Entities;
using Avatar.App.Infrastructure.FileStorage.Interfaces;
using Avatar.App.Infrastructure.Models.Semifinal;

namespace Avatar.App.Infrastructure.Repositories
{
    public class BattleSemifinalistRepository : EfBaseRepository<BattleSemifinalistDb>
    {
        public BattleSemifinalistRepository(AvatarAppContext dbContext, IStorageService storageService) : base(dbContext, storageService)
        {
        }
    }
}
