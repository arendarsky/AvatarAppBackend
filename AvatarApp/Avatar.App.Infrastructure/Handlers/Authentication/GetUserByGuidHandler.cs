using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Avatar.App.Casting.Commands;
using Avatar.App.Casting.Models;
using Avatar.App.Infrastructure.Handlers.Abstract;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Avatar.App.Infrastructure.Handlers.Authentication
{
    internal class GetUserByGuidHandler: AutoMapperEFHandler, IRequestHandler<GetUserByGuid, Contestant>
    {
        public GetUserByGuidHandler(AvatarAppContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<Contestant> Handle(GetUserByGuid request, CancellationToken cancellationToken)
        {
            return Mapper.Map<Contestant>(await DbContext.Users.FirstOrDefaultAsync(user => user.Guid == request.Guid,
                cancellationToken));
        }
    }
}
