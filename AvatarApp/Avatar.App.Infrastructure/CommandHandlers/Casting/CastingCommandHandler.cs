namespace Avatar.App.Infrastructure.CommandHandlers.Casting
{
    public abstract class CastingCommandHandler
    {
        protected AvatarAppContext DbContext;

        protected CastingCommandHandler(AvatarAppContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}
