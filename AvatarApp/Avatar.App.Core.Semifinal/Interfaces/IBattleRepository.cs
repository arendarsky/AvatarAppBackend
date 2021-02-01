using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Avatar.App.Core.Entities;
using Avatar.App.SharedKernel.Interfaces;

namespace Avatar.App.Core.Semifinal.Interfaces
{
    public interface IBattleRepository: IRepository<Battle>
    {
        IQueryable<Battle> GetWithRelations(Expression<Func<Battle, bool>> predicate);
    }
}
