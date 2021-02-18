using Avatar.App.SharedKernel;
using MediatR;

namespace Avatar.App.Final
{
    public interface IFinalComponent
    {
    }

    internal class FinalComponent: AvatarAppComponent, IFinalComponent
    {
        public FinalComponent(IMediator mediator, IQueryManager queryManager) : base(mediator, queryManager)
        {
        }
    }
}
