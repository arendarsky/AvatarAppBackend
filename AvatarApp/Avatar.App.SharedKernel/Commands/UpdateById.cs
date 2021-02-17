using MediatR;

namespace Avatar.App.SharedKernel.Commands
{
    public class UpdateById<T>: ByIdCommand, IRequest
    {
        public UpdateById(long id) : base(id)
        {
        }
    }
}
