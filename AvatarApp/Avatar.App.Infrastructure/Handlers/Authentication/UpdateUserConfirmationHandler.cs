using System.Threading;
using System.Threading.Tasks;
using Avatar.App.Authentication.Commands;
using Avatar.App.Infrastructure.Handlers.Abstract;
using Avatar.App.Infrastructure.Models.Casting;
using MediatR;

namespace Avatar.App.Infrastructure.Handlers.Authentication
{
    internal class UpdateUserConfirmationHandler: EFHandler, IRequestHandler<UpdateUserConfirmation>
    {
        public UpdateUserConfirmationHandler(AvatarAppContext dbContext) : base(dbContext)
        {
        }

        public async Task<Unit> Handle(UpdateUserConfirmation request, CancellationToken cancellationToken)
        {
            var user = request.User;
            var userDb = new UserDb {Id = user.Id};
            userDb.UpdateProperty(DbContext, nameof(userDb.IsEmailConfirmed), user.IsEmailConfirmed);
            userDb.UpdateProperty(DbContext, nameof(userDb.ConfirmationCode), user.ConfirmationCode);
            await DbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
