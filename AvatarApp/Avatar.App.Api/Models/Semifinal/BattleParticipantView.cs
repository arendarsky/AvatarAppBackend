using Avatar.App.Api.Models.Common;
using Avatar.App.Semifinal.Models;

namespace Avatar.App.Api.Models.Semifinal
{
    public class BattleParticipantView: BaseContestantView
    {
        public SemifinalistView Semifinalist { get; set; }

        public BattleParticipantView(Semifinalist semifinalist, long userId, long battleId) : base(semifinalist.Contestant)
        {
            Semifinalist = new SemifinalistView(semifinalist, userId, battleId);
        }
    }
}
