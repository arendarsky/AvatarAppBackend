using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Avatar.App.Final.Commands;
using Avatar.App.Final.Models;
using Avatar.App.Infrastructure.Handlers.Abstract;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Avatar.App.Infrastructure.Handlers.Final
{
    internal class GetActiveFinalHandler: AutoMapperEFHandler, IRequestHandler<GetActiveFinal, App.Final.Models.Final>
    {
        public GetActiveFinalHandler(AvatarAppContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<App.Final.Models.Final> Handle(GetActiveFinal request, CancellationToken cancellationToken)
        {
            var finalQuery = DbContext.Finals.Where(fin => fin.IsActive);
            var final = await Mapper.ProjectTo<App.Final.Models.Final>(finalQuery)
                .FirstOrDefaultAsync(cancellationToken);
            if (final == null) return null;
            var finalists = await Mapper.ProjectTo<Finalist>(DbContext.Finalists.AsQueryable())
                .ToListAsync(cancellationToken);
            final.Finalists = finalists;
            return final;
        }
    }
}
