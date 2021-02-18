

using System.Linq;
using Avatar.App.Casting.Models;
using Avatar.App.Infrastructure.Models.Casting;
using Avatar.App.Rating.Models;

namespace Avatar.App.Infrastructure.AutoMapperProfiles
{
    internal class RatingProfile : AutoMapper.Profile
    {
        public RatingProfile()
        {
            CreateMap<UserDb, RatingContestantPerformance>().IncludeBase<UserDb, ContestantPerformance>()
                .ForMember(dest => dest.LikesNumber,
                    opt => opt.MapFrom(src => src.LoadedVideos.Sum(video => video.WatchedBy.Count(view => view.IsLiked))))
                .ForMember(dest => dest.IsSemifinalist,
                    opt => opt.MapFrom(src => src.Semifinalist != null));

            CreateMap<UserDb, RatingSemifinalist>()
                .ForMember(dest => dest.LikesNumber,
                    opt => opt.MapFrom(
                        src => src.LoadedVideos.Sum(video => video.WatchedBy.Count(view => view.IsLiked))));
            CreateMap<UserDb, RatingFinalist>()
                .ForMember(dest => dest.LikesNumber, opt => opt.MapFrom(src => src.Semifinalist.Votes.Count));
        }
    }
}
