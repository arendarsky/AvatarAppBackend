using System;
using Avatar.App.Authentication.Models;
using MediatR;

namespace Avatar.App.Authentication.Commands
{
    public class GetUserByGuid : IRequest<User>
    {
        public Guid Guid { get; }

        public GetUserByGuid(Guid guid)
        {
            Guid = guid;
        }
    }
}
