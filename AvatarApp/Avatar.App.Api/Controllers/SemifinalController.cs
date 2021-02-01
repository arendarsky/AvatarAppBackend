using Avatar.App.Core.Entities;
using Avatar.App.Core.Models;
using Avatar.App.Core.Services;
using Avatar.App.SharedKernel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Api.Models;
using Avatar.App.Core.Semifinal.DTO;
using Avatar.App.Core.Semifinal.Interfaces;
using Avatar.App.Semifinal;

namespace Avatar.App.Api.Controllers
{
    [Authorize]
    [Route("api/semifinal")]
    [ApiController]
    public class SemifinalController : BaseAuthorizeController
    {
        private readonly IBattleService _battleService;
        private readonly IBattleVoteService _battleVoteService;
        private readonly ISemifinalComponent _semifinalComponent;

        public SemifinalController(IBattleService battleService, IBattleVoteService battleVoteService, ISemifinalComponent semifinalComponent)
        {
            _battleService = battleService;
            _battleVoteService = battleVoteService;
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
        public async Task<BattleParticipantVotesDTO> VoteTo(BattleVoteDTO battleVoteDTO)
        {
            var userGuid = GetUserGuid();
            return await _battleVoteService.VoteToAsync(userGuid, battleVoteDTO);
        }
    }
}
