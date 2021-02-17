using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Avatar.App.Infrastructure.Handlers.Abstract;
using Avatar.App.SharedKernel.Commands;
using MediatR;

namespace Avatar.App.Infrastructure.Handlers.Common.Generic
{
    internal class GetQueryHandler<TSource, TDestination>: AutoMapperEFHandler, IRequestHandler<GetQuery<TDestination>, IQueryable<TDestination>>, IGenericHandler where TSource: class
    {
        public GetQueryHandler(AvatarAppContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public Task<IQueryable<TDestination>> Handle(GetQuery<TDestination> request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Mapper.ProjectTo<TDestination>(DbContext.Set<TSource>().AsQueryable()));
        }
    }
}
