using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Avatar.App.SharedKernel.Commands
{
    public class RemoveById<T>: IRequest
    {
        public long Id { get; }

        public RemoveById(long id)
        {
            Id = id;
        }
    }
}
