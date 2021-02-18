using MediatR;

namespace Avatar.App.SharedKernel
{
    public abstract class AvatarAppComponent
    {
        protected IQueryManager QueryManager;
        protected IMediator Mediator;

        protected AvatarAppComponent(IMediator mediator, IQueryManager queryManager)
        {
            QueryManager = queryManager;
            Mediator = mediator;
        }
    }
}
