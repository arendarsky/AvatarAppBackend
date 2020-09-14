using System;
using System.Threading.Tasks;
using Avatar.App.Core.Semifinal.DTO;

namespace Avatar.App.Core.Semifinal.Interfaces
{
    public interface IBattleVoteService
    {
        Task VoteTo(Guid userGuid, BattleVoteCreationDTO battleVoteCreationDTO);
        Task CancelVote(Guid userGuid, BattleVoteCreationDTO battleVoteCreationDTO);
    }
}
