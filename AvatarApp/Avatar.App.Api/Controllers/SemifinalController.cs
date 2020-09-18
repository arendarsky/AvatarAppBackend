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

namespace Avatar.App.Api.Controllers
{
    [Authorize]
    [Route("api/semifinal")]
    [ApiController]
    public class SemifinalController : BaseAuthorizeController
    {
        private readonly IBattleService _battleService;
        private readonly IBattleVoteService _battleVoteService;

        public SemifinalController(IBattleService battleService, IBattleVoteService battleVoteService)
        {
            _battleService = battleService;
            _battleVoteService = battleVoteService;
        }

        /// <summary>
        /// Returns active battles
        /// </summary>
        [SwaggerOperation("GetBattles")]
        [Route("battles/get")]
        [HttpGet]
        public ActionResult GetBattles()
        {
            var userGuid = GetUserGuid();
            var battles =  _battleService.GetBattles();
            var battleModels = battles.Select(battle => BattleModel.FromBattleWithUserLikeInfo(battle, userGuid));
            return new JsonResult(battleModels);
        }

        /// <summary>
        /// Set vote 
        /// </summary>
        [SwaggerOperation("VoteTo")]
        [Route("vote")]
        [HttpPost]
        public async Task VoteTo(BattleVoteDTO battleVoteDTO)
        {
            var userGuid = GetUserGuid();
            await _battleVoteService.VoteToAsync(userGuid, battleVoteDTO);
        }

        /// <summary>
        /// Cancel vote
        /// </summary>
        [SwaggerOperation("CancelVote")]
        [Route("vote/cancel")]
        [HttpPost]
        public async Task CancelVote(BattleVoteDTO battleVoteDTO)
        {
            var userGuid = GetUserGuid();
            await _battleVoteService.CancelVoteAsync(userGuid, battleVoteDTO);
        }
    }
}
