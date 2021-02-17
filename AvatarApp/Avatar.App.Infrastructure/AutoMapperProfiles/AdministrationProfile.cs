using Avatar.App.Administration.Models;
using Avatar.App.Casting.Models;
using Avatar.App.Infrastructure.Models.Casting;
using Video = Avatar.App.Administration.Models.Video;

namespace Avatar.App.Infrastructure.AutoMapperProfiles
{
    internal class AdministrationProfile: AutoMapper.Profile
    {
        public AdministrationProfile()
        {
            CreateMap<VideoDb, Video>();
            CreateMap<UserDb, ModerationContestantPerformance>().IncludeBase<UserDb, ContestantPerformance>();
        }
    }
}
