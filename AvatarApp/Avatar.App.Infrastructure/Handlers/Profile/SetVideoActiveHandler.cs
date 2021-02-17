using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avatar.App.Infrastructure.Handlers.Abstract;
using Avatar.App.Profile.Commands;
using Avatar.App.Profile.Models;
using Avatar.App.SharedKernel.Commands;
using MediatR;

namespace Avatar.App.Infrastructure.Handlers.Profile
{
    internal class SetVideoActiveHandler : EFHandler, IRequestHandler<SetVideoActive>
    {
        private readonly IMediator _mediator;

        public SetVideoActiveHandler(AvatarAppContext dbContext, IMediator mediator) : base(dbContext)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(SetVideoActive request, CancellationToken cancellationToken)
        {
            var user = await _mediator.Send(new GetUserByGuid<PrivateContestantProfile>(request.UserGuid), cancellationToken);
            var video = user.Videos.FirstOrDefault(vid =>
                string.Equals(vid.Name, request.FileName, StringComparison.Ordinal));

            if (video == null || video.IsActive || !video.IsApproved.HasValue || !video.IsApproved.Value)
            {
                return Unit.Value;
            }

            foreach (var activeVideo in user.Videos.Where(vid => vid.IsActive))
            {
                activeVideo.IsActive = false;
            }

            video.IsActive = true;
            await DbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
