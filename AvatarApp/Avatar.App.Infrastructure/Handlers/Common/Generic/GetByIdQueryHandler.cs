using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Avatar.App.Infrastructure.Handlers.Abstract;
using Avatar.App.Infrastructure.Models;
using Avatar.App.SharedKernel.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Avatar.App.Infrastructure.Handlers.Common.Generic
{
    internal class GetByIdQueryHandler<TSource, TDestination>: AutoMapperEFHandler, IRequestHandler<GetById<TDestination>, TDestination>, IGenericHandler where TSource: BaseEntity
    {
        public GetByIdQueryHandler(AvatarAppContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<TDestination> Handle(GetById<TDestination> request, CancellationToken cancellationToken)
        {
            var query = Mapper.ProjectTo<TDestination>(DbContext.Set<TSource>().Where(item => item.Id == request.Id));
            return await query.FirstOrDefaultAsync(cancellationToken);
        }
    }
}
