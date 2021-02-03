using System;
using Avatar.App.Semifinal.CData;
using MediatR;

namespace Avatar.App.Semifinal.Commands
{
    public class ToVoteInBattle: IRequest
    {
        public BattleVoteCData VoteDTO { get; }
        public Guid UserGuid { get; }

        public ToVoteInBattle(BattleVoteCData voteDTO, Guid userGuid)
        {
            VoteDTO = voteDTO;
            UserGuid = userGuid;
        }
    }
}
