using System;
using MediatR;

namespace Avatar.App.Casting.Commands
{
    public class RemoveVideo: IRequest
    {
        public Guid UserGuid { get; }
        public string FileName { get; }

        public RemoveVideo(Guid userGuid, string fileName)
        {
            UserGuid = userGuid;
            FileName = fileName;
        }
    }
}
