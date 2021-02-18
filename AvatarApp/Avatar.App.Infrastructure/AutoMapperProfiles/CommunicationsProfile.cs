using Avatar.App.Communications.Models;
using Avatar.App.Infrastructure.Models.Casting;

namespace Avatar.App.Infrastructure.AutoMapperProfiles
{
    internal class CommunicationsProfile: AutoMapper.Profile
    {
        public CommunicationsProfile()
        {
            CreateMap<UserDb, NotificationAuthor>();
            CreateMap<WatchedVideoDb, LikeNotification>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.User));
        }
    }
}
