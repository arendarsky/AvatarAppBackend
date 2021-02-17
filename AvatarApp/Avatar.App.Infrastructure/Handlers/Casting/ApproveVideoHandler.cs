using System;
using System.Threading;
using System.Threading.Tasks;
using Avatar.App.Administration.Commands;
using Avatar.App.Infrastructure.Handlers.Abstract;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Avatar.App.Infrastructure.Handlers.Casting
{
    internal class ApproveVideoHandler: EFHandler, IRequestHandler<ApproveVideo>
    {
        public ApproveVideoHandler(AvatarAppContext dbContext) : base(dbContext)
        {
        }

        public async Task<Unit> Handle(ApproveVideo request, CancellationToken cancellationToken)
        {
            var videoDb = await DbContext.Videos.FirstOrDefaultAsync(
                video => string.Equals(video.Name, request.VideoName, StringComparison.Ordinal), cancellationToken);
            videoDb.IsApproved = request.IsApproved;
            await DbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
