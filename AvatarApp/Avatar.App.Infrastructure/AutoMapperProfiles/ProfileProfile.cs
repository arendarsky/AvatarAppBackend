using System.Linq;
using Avatar.App.Infrastructure.Models.Casting;
using Avatar.App.Profile.Models;

namespace Avatar.App.Infrastructure.AutoMapperProfiles
{
    internal class ProfileProfile: AutoMapper.Profile
    {
        public ProfileProfile()
        {
            CreateMap<VideoDb, PublicProfileVideo>();
            CreateMap<VideoDb, PrivateProfileVideo>();
            CreateMap<UserDb, PrivateContestantProfile>()
                .ForMember(dest => dest.Videos, opt => opt.MapFrom(src => src.LoadedVideos));
            CreateMap<UserDb, PublicContestantProfile>()
                .ForMember(dest => dest.Videos, opt => opt.MapFrom(src => src.LoadedVideos.Where(video => video.IsApproved.HasValue && video.IsApproved.Value)));
        }
    }
}
