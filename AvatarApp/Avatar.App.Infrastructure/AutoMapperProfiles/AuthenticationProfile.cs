using Avatar.App.Authentication.CData;
using Avatar.App.Authentication.Models;
using Avatar.App.Infrastructure.Models.Casting;

namespace Avatar.App.Infrastructure.AutoMapperProfiles
{
    internal class AuthenticationProfile: AutoMapper.Profile
    {
        public AuthenticationProfile()
        {
            CreateMap<UserDb, User>();
            CreateMap<UserCData, UserDb>();
        }
    }
}
