using System.Threading;
using System.Threading.Tasks;
using Avatar.App.Casting.Commands;
using Avatar.App.Infrastructure.Handlers.Abstract;
using Avatar.App.Infrastructure.Models.Casting;
using MediatR;

namespace Avatar.App.Infrastructure.Handlers.Casting
{
    internal class UpdateVideoFragmentIntervalHandler: EFHandler, IRequestHandler<UpdateVideoFragmentInterval>
    {
        public UpdateVideoFragmentIntervalHandler(AvatarAppContext dbContext) : base(dbContext)
        {
        }

        public async Task<Unit> Handle(UpdateVideoFragmentInterval request, CancellationToken cancellationToken)
        {
            var interval = request.Interval;
            var videoDb =
                await VideoDb.GetByNameAndUserGuidAsync(DbContext, request.UserGuid, interval.FileName, cancellationToken);
            if (videoDb == null) return Unit.Value;
            videoDb.UpdateProperty(DbContext, nameof(videoDb.StartTime), interval.StartTime);
            videoDb.UpdateProperty(DbContext, nameof(videoDb.EndTime), interval.EndTime);
            await DbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
