using System.Linq;
using MediatR;

namespace Avatar.App.SharedKernel.Commands
{
    public class GetQuery<T>: IRequest<IQueryable<T>>
    {
    }
}
