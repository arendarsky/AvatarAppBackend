using Avatar.App.Core.Entities;
using Avatar.App.Infrastructure.FileStorage.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Avatar.App.Infrastructure.Repositories
{
    public class BattleRepository : EfBaseRepository<Battle>
    {
        public BattleRepository(AvatarAppContext dbContext, IStorageService storageService) : base(dbContext, storageService)
        {
        }
    }
}
