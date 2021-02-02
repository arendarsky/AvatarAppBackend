using System;
using System.Collections.Generic;
using System.Text;

namespace Avatar.App.Infrastructure.CommandHandlers.Basic
{
    internal abstract class EFCommandHandler
    {
        protected AvatarAppContext DbContext;

        protected EFCommandHandler(AvatarAppContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}
