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
                .ForMember(dest => dest.Videos, opt => opt.MapFrom(src => src.LoadedVideos))
                .ForMember(dest => dest.LikesNumber,
                    opt => opt.MapFrom(
                        src => src.LoadedVideos.Sum(video => video.WatchedBy.Count(view => view.IsLiked))));
            CreateMap<UserDb, PublicContestantProfile>()
                .ForMember(dest => dest.Videos, opt => opt.MapFrom(src => src.LoadedVideos.Where(video => video.IsApproved.HasValue && video.IsApproved.Value)));
        }
    }
}
