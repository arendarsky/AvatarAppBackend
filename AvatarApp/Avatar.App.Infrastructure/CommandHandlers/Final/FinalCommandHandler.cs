namespace Avatar.App.Infrastructure.CommandHandlers.Final
{
    internal class FinalCommandHandler
    {
        protected readonly AvatarAppContext DbContext;

        public FinalCommandHandler(AvatarAppContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}
