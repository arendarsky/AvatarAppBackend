using Avatar.App.Core.Entities;
using Avatar.App.Infrastructure.FileStorage.Interfaces;

namespace Avatar.App.Infrastructure.Repositories
{
    public class BattleVoteRepository : EfBaseRepository<BattleVote>
    {
        public BattleVoteRepository(AvatarAppContext dbContext, IStorageService storageService) : base(dbContext, storageService)
        {
        }
    }
}
