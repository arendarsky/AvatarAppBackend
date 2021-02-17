using System;
using Avatar.App.Casting.Models;
using MediatR;

namespace Avatar.App.Casting.Commands
{
    public class UpdateVideoFragmentInterval: IRequest
    {
        public VideoFragmentUpdate Interval { get; }
        public Guid UserGuid { get; }

        public UpdateVideoFragmentInterval(VideoFragmentUpdate interval, Guid userGuid)
        {
            Interval = interval;
            UserGuid = userGuid;
        }
    }
}
