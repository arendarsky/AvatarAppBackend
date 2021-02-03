using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Api.Models;
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

        /// <summary>
        /// Returns active battles
        /// </summary>
        [SwaggerOperation("GetBattles")]
        [Route("battles/get")]
        [HttpGet]
        public async Task<IEnumerable<BattleModel>> GetBattles()
        {
            var userGuid = GetUserGuid();
            var battles = await _semifinalComponent.GetBattles();
            return battles.Select(battle => BattleModel.FromBattleWithUserLikeInfo(battle, userGuid));
        }

        /// <summary>
        /// Set vote 
        /// </summary>
        [SwaggerOperation("VoteTo")]
        [Route("vote")]
        [HttpPost]
        public async Task<BattleParticipantsVote> VoteTo(BattleVoteCData battleVoteCData)
        {
            var user = await GetUser();
            battleVoteCData.UserId = user.Id;
            return new BattleParticipantsVote(await _semifinalComponent.VoteTo(battleVoteCData));
        }
    }
}
