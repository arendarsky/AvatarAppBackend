using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Avatar.App.Infrastructure.Handlers.Abstract;
using Avatar.App.Infrastructure.Models.Casting;
using Avatar.App.SharedKernel.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Avatar.App.Infrastructure.Handlers.Common.Generic
{
    internal class GetUserByGuidHandler<TSource, TDestination>: AutoMapperEFHandler, IRequestHandler<GetUserByGuid<TDestination>, TDestination>, IGenericHandler<UserDb> where TSource: UserDb
    {
        public GetUserByGuidHandler(AvatarAppContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<TDestination> Handle(GetUserByGuid<TDestination> request, CancellationToken cancellationToken)
        {
            var userQuery = Mapper.ProjectTo<TDestination>(DbContext.Set<TSource>().Where(user => user.Guid == request.Guid));
            return await userQuery.FirstOrDefaultAsync(cancellationToken);
        }
    }
}
