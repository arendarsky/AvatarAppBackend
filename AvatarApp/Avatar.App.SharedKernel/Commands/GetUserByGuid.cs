using System;
using MediatR;

namespace Avatar.App.SharedKernel.Commands
{
    public class GetUserByGuid<TUser>: IRequest<TUser>
    {
        public GetUserByGuid(Guid guid)
        {
            Guid = guid;
        }

        public Guid Guid { get; }
    }
}
