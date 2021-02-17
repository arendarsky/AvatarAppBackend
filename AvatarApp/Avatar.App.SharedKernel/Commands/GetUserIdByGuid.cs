using System;
using MediatR;

namespace Avatar.App.SharedKernel.Commands
{
    public class GetUserIdByGuid: IRequest<long>
    {
        public Guid UserGuid { get; }


        public GetUserIdByGuid(Guid userGuid)
        {
            UserGuid = userGuid;
        }
    }
}
