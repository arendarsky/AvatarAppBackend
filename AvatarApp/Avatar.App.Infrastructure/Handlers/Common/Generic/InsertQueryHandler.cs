using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Avatar.App.Infrastructure.Handlers.Abstract;
using Avatar.App.SharedKernel.Commands;
using MediatR;

namespace Avatar.App.Infrastructure.Handlers.Common.Generic
{
    internal class InsertQueryHandler<TSource, TDestination> : AutoMapperEFHandler, IRequestHandler<Insert<TSource>, Unit>, IGenericInsertHandler where TDestination : class
    {
        public InsertQueryHandler(AvatarAppContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<Unit> Handle(Insert<TSource> request, CancellationToken cancellationToken)
        {
            var dbItem = Mapper.Map<TDestination>(request.Item);
            await DbContext.Set<TDestination>().AddAsync(dbItem, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
