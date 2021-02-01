using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Avatar.App.Semifinal.Models;

namespace Avatar.App.Semifinal
{
    internal class SemifinalProfile: Profile
    {
        public SemifinalProfile()
        {
            CreateMap<Core.Entities.Semifinalist, Semifinalist>()
                .ForMember(dest => dest.Contestant, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.Votes, opt => opt.MapFrom(src => src.Votes.Select(vote => vote.User)));

            CreateMap<Core.Entities.Battle, Battle>()
                .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.BattleSemifinalists.Select(batSem => batSem.Semifinalist)));
        }
    }
}
