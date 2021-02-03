using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Avatar.App.SharedKernel.Commands
{
    public class Insert<T>: IRequest
    {
        public T Item { get; }

        public Insert(T item)
        {
            Item = item;
        }
    }
}
