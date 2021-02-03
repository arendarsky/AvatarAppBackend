using AutoMapper;
using Avatar.App.Casting.Models;
using Avatar.App.Infrastructure.Models.Casting;

namespace Avatar.App.Infrastructure.AutoMapperProfiles
{
    internal class CastingProfile: Profile
    {
        public CastingProfile()
        {
            CreateMap<UserDb, Contestant>();
        }
    }
}
