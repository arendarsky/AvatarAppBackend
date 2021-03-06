﻿using MediatR;

namespace Avatar.App.Semifinal.Commands
{
    public class CloseBattle: IRequest
    {
        public CloseBattle(long battleId)
        {
            BattleId = battleId;
        }

        public long BattleId { get; }
    }
}
