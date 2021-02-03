using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Avatar.App.Infrastructure.Handlers.Abstract;
using Avatar.App.SharedKernel.Commands;
using MediatR;

namespace Avatar.App.Infrastructure.Handlers.Generic
{
    internal class GetByIdHandler<TSource, TDestination>: AutoMapperEFHandler, IRequestHandler<GetById<TDestination>, TDestination>, IGenericQueryHandler where TSource: class
    {
        public GetByIdHandler(AvatarAppContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<TDestination> Handle(GetById<TDestination> request, CancellationToken cancellationToken)
        {
            return Mapper.Map<TDestination>(await DbContext.Set<TSource>().FindAsync(request.Id, cancellationToken));
        }
    }
}
