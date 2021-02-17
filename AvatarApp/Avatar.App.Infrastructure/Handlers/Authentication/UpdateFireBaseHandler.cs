using System.Threading;
using System.Threading.Tasks;
using Avatar.App.Authentication.Commands;
using Avatar.App.Infrastructure.Handlers.Abstract;
using Avatar.App.Infrastructure.Models.Casting;
using MediatR;

namespace Avatar.App.Infrastructure.Handlers.Authentication
{
    internal class UpdateFireBaseHandler: EFHandler, IRequestHandler<UpdateFireBase>
    {
        public UpdateFireBaseHandler(AvatarAppContext dbContext) : base(dbContext)
        {
        }

        public async Task<Unit> Handle(UpdateFireBase request, CancellationToken cancellationToken)
        {
            var userDb = new UserDb {Id = request.User.Id};
            userDb.UpdateProperty(DbContext, nameof(userDb.FireBaseId), request.User.FireBaseId);
            await DbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
