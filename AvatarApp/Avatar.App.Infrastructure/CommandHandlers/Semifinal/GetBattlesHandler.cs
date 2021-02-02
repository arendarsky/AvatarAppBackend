﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Avatar.App.Infrastructure.CommandHandlers.Basic;
using Avatar.App.Semifinal.Commands;
using Avatar.App.Semifinal.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Avatar.App.Infrastructure.CommandHandlers.Semifinal
{
    internal class GetBattlesHandler: AutoMapperEFCommandHandler, IRequestHandler<GetBattles, IEnumerable<Battle>>
    {
        public GetBattlesHandler(AvatarAppContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<IEnumerable<Battle>> Handle(GetBattles request, CancellationToken cancellationToken)
        {
            return await Mapper.ProjectTo<Battle>(DbContext.Battles).ToListAsync(cancellationToken);
        }
    }
}
