using System.Threading;
using System.Threading.Tasks;
using Avatar.App.Authentication.Commands;
using Avatar.App.Infrastructure.Handlers.Abstract;
using Avatar.App.Infrastructure.Models.Casting;
using MediatR;

namespace Avatar.App.Infrastructure.Handlers.Authentication
{
    internal class ChangePasswordHandler: EFHandler, IRequestHandler<ChangePassword>
    {
        public ChangePasswordHandler(AvatarAppContext dbContext) : base(dbContext)
        {
        }

        public async Task<Unit> Handle(ChangePassword request, CancellationToken cancellationToken)
        {
            var userDb = new UserDb {Id = request.User.Id};
            userDb.UpdateProperty(DbContext, nameof(userDb.ConfirmationCode), request.User.ConfirmationCode);
            userDb.UpdateProperty(DbContext, nameof(userDb.Password), request.User.Password);
            await DbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
