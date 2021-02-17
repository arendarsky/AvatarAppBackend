using System.Threading;
using System.Threading.Tasks;
using Avatar.App.Authentication.Commands;
using Avatar.App.Authentication.Models;
using Avatar.App.Infrastructure.Handlers.Abstract;
using Avatar.App.SharedKernel.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Avatar.App.Infrastructure.Handlers.Authentication
{
    internal class GetUserByEmailHandler: EFHandler, IRequestHandler<GetUserByEmail, User>
    {
        private readonly IMediator _mediator;

        public GetUserByEmailHandler(AvatarAppContext dbContext, IMediator mediator) : base(dbContext)
        {
            _mediator = mediator;
        }

        public async Task<User> Handle(GetUserByEmail request, CancellationToken cancellationToken)
        {
            var userQuery = await _mediator.Send(new GetQuery<User>(), cancellationToken);
            return await userQuery.FirstOrDefaultAsync(user =>
                string.Equals(user.Email, request.Email), cancellationToken);
        }
    }
}
