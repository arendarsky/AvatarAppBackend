using System.Threading;
using System.Threading.Tasks;
using Avatar.App.Infrastructure.Handlers.Abstract;
using Avatar.App.Infrastructure.Models.Semifinal;
using Avatar.App.Semifinal.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Avatar.App.Infrastructure.Handlers.Semifinal
{
    internal sealed class ToVoteInBattleHandler: EFHandler, IRequestHandler<ToVoteInBattle>
    {
        public ToVoteInBattleHandler(AvatarAppContext dbContext) : base(dbContext)
        {
        }

        public async Task<Unit> Handle(ToVoteInBattle request, CancellationToken cancellationToken)
        {
            var voteDTO = request.VoteDTO;
            var existedVote = await DbContext.BattleVotes.FirstOrDefaultAsync(
                vote => vote.User.Guid == request.UserGuid && vote.BattleId == voteDTO.BattleId &&
                        vote.SemifinalistId == voteDTO.SemifinalistId, cancellationToken);

            if (existedVote != null)
            {
                DbContext.BattleVotes.Remove(existedVote);
            }
            else
            {
                var user = await DbContext.Users.FirstOrDefaultAsync(us => us.Guid == request.UserGuid,
                    cancellationToken);
                var newVote = new BattleVoteDb
                {
                    BattleId = voteDTO.BattleId,
                    SemifinalistId = voteDTO.SemifinalistId,
                    UserId = user.Id
                };
                await DbContext.BattleVotes.AddAsync(newVote, cancellationToken);
            }

            await DbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
