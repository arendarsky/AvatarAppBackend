using AutoMapper;
using Avatar.App.Casting.Models;
using Avatar.App.Core.Entities;

namespace Avatar.App.Infrastructure.AutoMapperProfiles
{
    internal class CastingProfile: Profile
    {
        public CastingProfile()
        {
            CreateMap<User, Contestant>();
        }
    }
}
