using AutoMapper;
using Avatar.App.Authentication.Models;

namespace Avatar.App.Infrastructure.AutoMapperProfiles
{
    internal class AuthenticationProfile: Profile
    {
        public AuthenticationProfile()
        {
            CreateMap<Models.Casting.UserDb, User>();
        }
    }
}
