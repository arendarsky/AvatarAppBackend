using System;
using System.Collections.Generic;
using System.Text;
using Avatar.App.Final.CreationData;
using Avatar.App.Final.Models;
using Avatar.App.Infrastructure.Models.Final;

namespace Avatar.App.Infrastructure.AutoMapperProfiles
{
    internal class FinalProfile: AutoMapper.Profile
    {
        public FinalProfile()
        {
            CreateMap<FinalDb, Final.Models.Final>().ForMember(dest => dest.Finalists, opt => opt.Ignore());
            CreateMap<FinalVoteDb, FinalVote>();
            CreateMap<FinalistDb, Finalist>().ForMember(dest => dest.Contestant, opt => opt.MapFrom(src => src.User));
            CreateMap<FinalVoteCreation, FinalVoteDb>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}
