using System;
using MediatR;

namespace Avatar.App.Profile.Commands
{
    public class UpdateProfilePhoto: IRequest
    {
        public UpdateProfilePhoto(string photoFileName, Guid userGuid)
        {
            PhotoFileName = photoFileName;
            UserGuid = userGuid;
        }

        public string PhotoFileName { get; }
        public Guid UserGuid { get; }
    }
}
