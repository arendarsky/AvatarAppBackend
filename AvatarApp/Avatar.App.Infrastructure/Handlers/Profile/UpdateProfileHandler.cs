using System.Threading;
using System.Threading.Tasks;
using Avatar.App.Infrastructure.Handlers.Abstract;
using Avatar.App.Infrastructure.Models.Casting;
using Avatar.App.Profile.Commands;
using MediatR;

namespace Avatar.App.Infrastructure.Handlers.Profile
{
    internal class UpdateProfileHandler: EFHandler, IRequestHandler<UpdateProfile>
    {
        public UpdateProfileHandler(AvatarAppContext dbContext) : base(dbContext)
        {
        }

        public async Task<Unit> Handle(UpdateProfile request, CancellationToken cancellationToken)
        {
            var profileUpdate = request.ProfileUpdate;
            var userDb = await UserDb.GetByGuidAsync(DbContext, request.UserGuid);
            userDb.Name = profileUpdate.Name;
            userDb.Description = profileUpdate.Description;
            userDb.InstagramLogin = profileUpdate.InstagramLogin;
            await DbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
