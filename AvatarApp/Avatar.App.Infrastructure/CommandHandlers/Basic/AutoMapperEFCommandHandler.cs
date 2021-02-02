using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace Avatar.App.Infrastructure.CommandHandlers.Basic
{
    internal abstract class AutoMapperEFCommandHandler: EFCommandHandler
    {
        protected IMapper Mapper;

        protected AutoMapperEFCommandHandler(AvatarAppContext dbContext, IMapper mapper) : base(dbContext)
        {
            Mapper = mapper;
        }
    }
}
