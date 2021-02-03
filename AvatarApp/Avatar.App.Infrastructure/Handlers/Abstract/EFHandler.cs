namespace Avatar.App.Infrastructure.Handlers.Abstract
{
    internal abstract class EFHandler
    {
        protected AvatarAppContext DbContext;

        protected EFHandler(AvatarAppContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}
