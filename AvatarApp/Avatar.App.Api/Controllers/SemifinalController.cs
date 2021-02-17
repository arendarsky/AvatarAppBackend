using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Api.Models.Semifinal;
using Avatar.App.Authentication;
using Avatar.App.Semifinal;
using Avatar.App.Semifinal.CData;

namespace Avatar.App.Api.Controllers
{
    [Authorize]
    [Route("api/semifinal")]
    [ApiController]
    public class SemifinalController : BaseAuthorizeController
    {
        private readonly ISemifinalComponent _semifinalComponent;

        public SemifinalController(IAuthenticationComponent authenticationComponent, ISemifinalComponent semifinalComponent): base(authenticationComponent)
        {
            _semifinalComponent = semifinalComponent;
        }

        [Route("battles/get")]
        [HttpGet]
        public async Task<IEnumerable<BattleView>> GetBattles()
        {
            var user = await GetUser();
            var battles = await _semifinalComponent.GetBattles();
            return battles.Select(battle =>new BattleView(battle, user.Id));
        }

        [Route("vote")]
        [HttpPost]
        public async Task<BattleVoteUpdateView> VoteTo(BattleVoteCData battleVoteCData)
        {
            var user = await GetUser();
            battleVoteCData.UserId = user.Id;
            return new BattleVoteUpdateView(await _semifinalComponent.VoteTo(battleVoteCData));
        }
    }
}
