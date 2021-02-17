

using System.Linq;
using Avatar.App.Casting.Models;
using Avatar.App.Infrastructure.Models.Casting;
using Avatar.App.Infrastructure.Models.Final;
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
                    opt => opt.MapFrom(src => src.LoadedVideos.Sum(video => video.WatchedBy.Count(view => view.IsLiked))))
                .ForAllMembers(opt => opt.PreCondition(src => src.Semifinalist != null));
            CreateMap<FinalistDb, RatingFinalist>()
                .ForMember(dest => dest.LikesNumber, opt => opt.MapFrom(src => src.User.Semifinalist.Votes.Count));
        }
    }
}
