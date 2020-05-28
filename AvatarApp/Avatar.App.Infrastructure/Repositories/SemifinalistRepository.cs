using System;
using System.Collections.Generic;
using System.Text;
using Avatar.App.Core.Entities;
using Avatar.App.Infrastructure.FileStorage.Interfaces;

namespace Avatar.App.Infrastructure.Repositories
{
    public class SemifinalistRepository: EfBaseRepository<Semifinalist>
    {
        public SemifinalistRepository(AvatarAppContext dbContext, IStorageService storageService) : base(dbContext, storageService)
        {
        }
    }
}
