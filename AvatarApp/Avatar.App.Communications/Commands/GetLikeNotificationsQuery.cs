using System;
using System.Linq;
using Avatar.App.Communications.Models;
using MediatR;

namespace Avatar.App.Communications.Commands
{
    public class GetLikeNotificationsQuery: IRequest<IQueryable<LikeNotification>>
    {
        public GetLikeNotificationsQuery(Guid userGuid)
        {
            UserGuid = userGuid;
        }

        public Guid UserGuid { get; }
    }
}
