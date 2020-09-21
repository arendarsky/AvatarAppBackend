using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Avatar.App.Core.Entities;

namespace Avatar.App.Core.Semifinal.Interfaces
{
    public interface ISemifinalistService
    {
        Semifinalist CreateFromUserId(long? userId);
        Task InsertSemifinalist(Semifinalist semifinalist);
    }
}
