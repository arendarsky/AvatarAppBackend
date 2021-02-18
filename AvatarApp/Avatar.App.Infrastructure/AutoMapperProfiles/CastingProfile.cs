using System.Linq;
using Avatar.App.Casting.CreationData;
using Avatar.App.Casting.Models;
using Avatar.App.Infrastructure.Models.Casting;
using Avatar.App.SharedKernel.Models;

namespace Avatar.App.Infrastructure.AutoMapperProfiles
{
    internal class CastingProfile: AutoMapper.Profile
    {
        public CastingProfile()
        {
            CreateMap<UserDb, Contestant>().IncludeBase<UserDb, BaseContestant>().ForMember(dest => dest.VideosNumber,
                opt => opt.MapFrom(src => src.LoadedVideos.Count));

            CreateMap<UserDb, ContestantPerformance>().IncludeBase<UserDb, BaseContestant>()
                .ForMember(dest => dest.ActiveVideo,
                    opt => opt.MapFrom(src => src.LoadedVideos.FirstOrDefault(video =>
                        video.IsActive && video.IsApproved.HasValue && video.IsApproved.Value)));

            CreateMap<VideoDb, Video>();
            CreateMap<VideoCreation, VideoDb>();
        }
    }
}
