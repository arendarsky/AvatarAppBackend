using System;
using Avatar.App.Profile.Models;
using MediatR;

namespace Avatar.App.Profile.Commands
{
    public class UpdateProfile: IRequest
    {
        public ProfileUpdate ProfileUpdate { get; }
        public Guid UserGuid { get; }

        public UpdateProfile(ProfileUpdate profileUpdate, Guid userGuid)
        {
            ProfileUpdate = profileUpdate;
            UserGuid = userGuid;
        }
    }
}
