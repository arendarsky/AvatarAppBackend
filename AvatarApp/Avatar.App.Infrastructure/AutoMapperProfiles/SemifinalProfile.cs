using System;
using System.Linq;
using Avatar.App.Infrastructure.Models.Semifinal;
using Avatar.App.Semifinal.CData;
using Avatar.App.Semifinal.Models;

namespace Avatar.App.Infrastructure.AutoMapperProfiles
{
    internal class SemifinalProfile: AutoMapper.Profile
    {
        public SemifinalProfile()
        {
            CreateMap<SemifinalistDb, Semifinalist>()
                .ForMember(dest => dest.Contestant, opt => opt.MapFrom(src => src.User));

            CreateMap<BattleDb, Battle>()
                .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.BattleSemifinalists.Select(batSem => batSem.Semifinalist)));

            CreateMap<BattleVoteDb, BattleVote>();

            CreateMap<BattleVoteDb, BattleVote>();

            CreateMap<BattleCData, BattleDb>()
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.BattleSemifinalists, opt => opt.MapFrom(src => src.SemifinalistsId));
            
            CreateMap<long, BattleSemifinalistDb>()
                .ForMember(dest => dest.SemifinalistId, opt => opt.MapFrom(src => src));

            CreateMap<BattleVoteCData, BattleVoteDb>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}
