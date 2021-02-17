using System;
using MediatR;

namespace Avatar.App.Profile.Commands
{
    public class SetVideoActive: IRequest
    {
        public string FileName { get; }
        public Guid UserGuid { get; }

        public SetVideoActive(string fileName, Guid userGuid)
        {
            FileName = fileName;
            UserGuid = userGuid;
        }
    }
}
