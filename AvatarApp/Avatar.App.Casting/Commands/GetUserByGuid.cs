using System;
using Avatar.App.Casting.Models;
using MediatR;

namespace Avatar.App.Casting.Commands
{
    public class GetUserByGuid: IRequest<Contestant>
    {
        public Guid Guid { get; }

        public GetUserByGuid(Guid guid)
        {
            Guid = guid;
        }
    }
}
