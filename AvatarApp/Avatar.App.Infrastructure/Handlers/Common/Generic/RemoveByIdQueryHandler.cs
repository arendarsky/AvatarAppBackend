using System.Threading;
using System.Threading.Tasks;
using Avatar.App.Infrastructure.Handlers.Abstract;
using Avatar.App.SharedKernel.Commands;
using MediatR;

namespace Avatar.App.Infrastructure.Handlers.Common.Generic
{
    internal class RemoveByIdQueryHandler<TSource, TDestination>: EFHandler, IRequestHandler<RemoveById<TDestination>, Unit>, IGenericHandler where TSource: class
    {

        public RemoveByIdQueryHandler(AvatarAppContext dbContext) : base(dbContext)
        {
  
        }

        public async Task<Unit> Handle(RemoveById<TDestination> request, CancellationToken cancellationToken)
        {
            DbContext.Set<TSource>().Remove(await DbContext.Set<TSource>().FindAsync(request.Id));
            await DbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
