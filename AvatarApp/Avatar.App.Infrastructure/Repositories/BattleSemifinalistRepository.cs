using Avatar.App.Core.Entities;
using Avatar.App.Infrastructure.FileStorage.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Avatar.App.Infrastructure.Repositories
{
    public class BattleSemifinalistRepository : EfBaseRepository<BattleSemifinalist>
    {
        public BattleSemifinalistRepository(AvatarAppContext dbContext, IStorageService storageService) : base(dbContext, storageService)
        {
        }
    }
}
