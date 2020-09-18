using System;
using System.Threading.Tasks;
using Avatar.App.Core.Semifinal.DTO;

namespace Avatar.App.Core.Semifinal.Interfaces
{
    public interface IBattleVoteService
    {
        Task VoteToAsync(Guid userGuid, BattleVoteDTO battleVoteCreationDTO);
        Task CancelVoteAsync(Guid userGuid, BattleVoteDTO battleVoteCreationDTO);
    }
}
