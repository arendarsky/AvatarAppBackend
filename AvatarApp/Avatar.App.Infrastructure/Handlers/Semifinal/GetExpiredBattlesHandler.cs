using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Avatar.App.Infrastructure.Handlers.Abstract;
using Avatar.App.Semifinal.Commands;
using Avatar.App.Semifinal.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Avatar.App.Infrastructure.Handlers.Semifinal
{
    internal class GetExpiredBattlesHandler: AutoMapperEFHandler, IRequestHandler<GetExpiredBattles, IEnumerable<Battle>>
    {
        public GetExpiredBattlesHandler(AvatarAppContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<IEnumerable<Battle>> Handle(GetExpiredBattles request, CancellationToken cancellationToken)
        {
            var nowTime = DateTime.Now;
            return await Mapper
                .ProjectTo<Battle>(DbContext.Battles.Where(battle => !battle.Closed && battle.EndDate <= nowTime))
                .ToListAsync(cancellationToken);
        }
    }
}
