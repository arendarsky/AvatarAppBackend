using MediatR;

namespace Avatar.App.SharedKernel
{
    public abstract class AvatarAppComponent
    {
        protected IMediator Mediator;

        protected AvatarAppComponent(IMediator mediator)
        {
            Mediator = mediator;
        }
    }
}
