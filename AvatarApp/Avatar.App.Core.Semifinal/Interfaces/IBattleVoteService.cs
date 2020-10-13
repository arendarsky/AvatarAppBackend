using System;
using System.Threading.Tasks;
using Avatar.App.Core.Semifinal.DTO;

namespace Avatar.App.Core.Semifinal.Interfaces
{
    public interface IBattleVoteService
    {
        Task<BattleParticipantVotesDTO> VoteToAsync(Guid userGuid, BattleVoteDTO battleVoteCreationDTO);
    }
}
