using System.Threading;
using System.Threading.Tasks;
using Avatar.App.Infrastructure.Handlers.Abstract;
using Avatar.App.Semifinal.CData;
using Avatar.App.Semifinal.Commands;
using Avatar.App.Semifinal.Models;
using Avatar.App.SharedKernel.Commands;
using MediatR;

namespace Avatar.App.Infrastructure.Handlers.Semifinal
{
    internal class UpdateVotesHandler: EFHandler, IRequestHandler<UpdateVotes>
    {
        private readonly IMediator _mediator;

        public UpdateVotesHandler(AvatarAppContext dbContext, IMediator mediator) : base(dbContext)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(UpdateVotes request, CancellationToken cancellationToken)
        {
            var votingRoom = request.VotingRoom;

            if (votingRoom.VoteToAdd != null)
            {
                await _mediator.Send(new Insert<BattleVoteCData>(votingRoom.VoteToAdd), cancellationToken);
            }

            if (votingRoom.VoteToRemove != null)
            {
                await _mediator.Send(new RemoveById<BattleVote>(votingRoom.VoteToRemove.Id), cancellationToken);
            }

            return Unit.Value;
        }
    }
}
