using MediatR;

namespace Avatar.App.SharedKernel.Commands
{
    public class RemoveById<T>: ByIdCommand, IRequest
    {
        public RemoveById(long id) : base(id)
        {
        }
    }
}
