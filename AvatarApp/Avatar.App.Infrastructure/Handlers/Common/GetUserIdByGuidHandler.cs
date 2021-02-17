using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avatar.App.Infrastructure.Handlers.Abstract;
using Avatar.App.SharedKernel.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Avatar.App.Infrastructure.Handlers.Common
{
    internal class GetUserIdByGuidHandler: EFHandler, IRequestHandler<GetUserIdByGuid, long>
    {
        public GetUserIdByGuidHandler(AvatarAppContext dbContext) : base(dbContext)
        {
        }

        public async Task<long> Handle(GetUserIdByGuid request, CancellationToken cancellationToken)
        {
            return await DbContext.Users.Where(user => user.Guid == request.UserGuid).Select(user => user.Id)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
