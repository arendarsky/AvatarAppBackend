using MediatR;

namespace Avatar.App.SharedKernel.Commands
{
    public class GetById<TDestination>: ByIdCommand, IRequest<TDestination>
    {
        public GetById(long id) : base(id)
        {
        }
    }
}
