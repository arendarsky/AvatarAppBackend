using System.Threading;
using System.Threading.Tasks;
using Avatar.App.Infrastructure.Handlers.Abstract;
using Avatar.App.Profile.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Avatar.App.Infrastructure.Handlers.Profile
{
    internal class UpdateProfilePhotoHandler: EFHandler, IRequestHandler<UpdateProfilePhoto>
    {
        public UpdateProfilePhotoHandler(AvatarAppContext dbContext) : base(dbContext)
        {
        }

        public async Task<Unit> Handle(UpdateProfilePhoto request, CancellationToken cancellationToken)
        {
            var userDb =
                await DbContext.Users.FirstOrDefaultAsync(user => user.Guid == request.UserGuid, cancellationToken);
            userDb.ProfilePhoto = request.PhotoFileName;
            await DbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
