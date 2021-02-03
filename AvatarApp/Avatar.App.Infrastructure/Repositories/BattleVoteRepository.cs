using Avatar.App.Infrastructure.FileStorage.Interfaces;
using Avatar.App.Infrastructure.Models.Semifinal;

namespace Avatar.App.Infrastructure.Repositories
{
    public class BattleVoteRepository: EfBaseRepository<BattleVoteDb>
    {
        public BattleVoteRepository(AvatarAppContext dbContext, IStorageService storageService) : base(dbContext, storageService)
        {
        }
    }
}
