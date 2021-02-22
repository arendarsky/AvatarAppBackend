using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avatar.App.Final.Commands;
using Avatar.App.Final.CreationData;
using Avatar.App.Infrastructure.Handlers.Abstract;
using Avatar.App.SharedKernel.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Avatar.App.Infrastructure.Handlers.Final
{
    internal class VoteInFinalHandler: EFHandler, IRequestHandler<VoteInFinal, bool>
    {
        private readonly IMediator _mediator;

        public VoteInFinalHandler(AvatarAppContext dbContext, IMediator mediator) : base(dbContext)
        {
            _mediator = mediator;
        }

        public async Task<bool> Handle(VoteInFinal request, CancellationToken cancellationToken)
        {
            var voteCreation = request.VoteCreation;
            var existedVote = await DbContext.FinalVotes.FirstOrDefaultAsync(
                vote => vote.UserId == voteCreation.UserId && vote.FinalistId == voteCreation.FinalistId,
                cancellationToken);

            if (existedVote != null)
            {
                DbContext.FinalVotes.Remove(existedVote);
                await DbContext.SaveChangesAsync(cancellationToken);
                return false;
            }

            var activeFinal = await DbContext.Finals.FirstOrDefaultAsync(final => final.IsActive, cancellationToken);
            var userVotesNumber =
                await DbContext.FinalVotes.CountAsync(vote => vote.UserId == voteCreation.UserId, cancellationToken);

            if (activeFinal.WinnersNumber <= userVotesNumber)
            {
                return false;
            }

            await _mediator.Send(new Insert<FinalVoteCreation>(voteCreation), cancellationToken);
            return true;
        }
    }
}
