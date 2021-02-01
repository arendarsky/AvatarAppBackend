using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Avatar.App.Core.Entities;
using Avatar.App.Core.Semifinal.Interfaces;
using Avatar.App.Infrastructure.FileStorage.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Avatar.App.Infrastructure.Repositories
{
    public class BattleRepository : EfBaseRepository<Battle>, IBattleRepository
    {
        public BattleRepository(AvatarAppContext dbContext, IStorageService storageService) : base(dbContext, storageService)
        {
        }

        public IQueryable<Battle> GetWithRelations(Expression<Func<Battle, bool>> predicate)
        {
            return DbContext.Battles
                .Include(battle => battle.BattleSemifinalists)
                .ThenInclude(battleSemifinalist => battleSemifinalist.Semifinalist)
                .ThenInclude(semifinalist => semifinalist.User)
                .Include(battle => battle.Votes)
                .ThenInclude(vote => vote.User)
                .Where(predicate);
        }
    }
}
