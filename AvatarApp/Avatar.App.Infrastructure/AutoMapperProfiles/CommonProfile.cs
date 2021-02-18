using Avatar.App.Infrastructure.Models.Casting;
using Avatar.App.SharedKernel.Models;

namespace Avatar.App.Infrastructure.AutoMapperProfiles
{
    internal class CommonProfile : AutoMapper.Profile
    {
        public CommonProfile()
        {
            CreateMap<UserDb, BaseContestant>();
        }
    }
}
