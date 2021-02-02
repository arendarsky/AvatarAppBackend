using System.Linq;
using AutoMapper;
using Avatar.App.Infrastructure.Models.Semifinal;
using Avatar.App.Semifinal.Models;

namespace Avatar.App.Infrastructure.AutoMapperProfiles
{
    internal class SemifinalProfile: Profile
    {
        public SemifinalProfile()
        {
            CreateMap<SemifinalistDb, Semifinalist>()
                .ForMember(dest => dest.Contestant, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.Votes, opt => opt.MapFrom(src => src.Votes.Select(vote => vote.User)));

            CreateMap<BattleDb, Battle>()
                .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.BattleSemifinalists.Select(batSem => batSem.Semifinalist)));
        }
    }
}
