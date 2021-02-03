using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediatR;

namespace Avatar.App.SharedKernel.Commands
{
    public class GetQuery<T>: IRequest<IQueryable<T>>
    {
    }
}
