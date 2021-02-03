using Avatar.App.Semifinal.CData;
using MediatR;

namespace Avatar.App.Semifinal.Commands
{
    public class AddBattle: IRequest
    {
        public AddBattle(BattleCData battleDTO)
        {
            BattleDTO = battleDTO;
        }

        public BattleCData BattleDTO { get; }
    }
}
