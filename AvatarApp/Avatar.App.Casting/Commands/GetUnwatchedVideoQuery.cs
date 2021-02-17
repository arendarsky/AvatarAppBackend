using System;
using System.Linq;
using Avatar.App.Casting.Models;
using MediatR;

namespace Avatar.App.Casting.Commands
{
    public class GetUnwatchedVideoQuery: IRequest<IQueryable<ContestantPerformance>>
    {
        public Guid UserGuid { get; }

        public GetUnwatchedVideoQuery(Guid userGuid)
        {
            UserGuid = userGuid;
        }
    }
}
