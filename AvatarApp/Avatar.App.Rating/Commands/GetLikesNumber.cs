using System;
using MediatR;

namespace Avatar.App.Rating.Commands
{
    public class GetLikesNumber: IRequest<int>
    {
        public GetLikesNumber(long userId)
        {
            UserId = userId;
        }

        public GetLikesNumber(Guid userGuid)
        {
            UserGuid = userGuid;
        }

        public long? UserId { get; }
        public Guid? UserGuid { get; }
    }
}
