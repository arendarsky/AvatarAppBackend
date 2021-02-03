using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Avatar.App.SharedKernel.Commands
{
    public class GetById<T>: IRequest<T>
    {
        public long Id { get; }

        public GetById(long id)
        {
            Id = id;
        }
    }
}
