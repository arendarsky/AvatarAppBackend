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

namespace Avatar.App.Api.Controllers
{
    [Authorize]
    [Route("api/semifinal")]
    [ApiController]
    public class SemifinalController : BaseAuthorizeController
    {
        private readonly ISemifinalService _semifinalService;

        public SemifinalController(ISemifinalService semifinalService)
        {
            _semifinalService = semifinalService;
        }


        /// <summary>
        /// Creates new battle 
        /// </summary>
        /// <param name="battleCreatingDto"></param>
        /// <response code="200">Returns bool value</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">If something goes wrong on server</response>
        [SwaggerOperation("Post")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "True if success")]
        [Route("battle")]
        [HttpPost]
        public async Task<ActionResult> CreateBattle(BattleCreatingDto battleCreatingDto)
        {
            try
            {
                var result = await _semifinalService.CreateBattleAsync(battleCreatingDto);
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                Logger.Log.LogError(ex.Message + ex.StackTrace);
                return Problem();
            }
        }


        /// <summary>
        /// Returns active battles
        /// </summary>
        /// <response code="200">Returns active battles</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">If something goes wrong on server</response>
        [SwaggerOperation("Get")]
        [SwaggerResponse(statusCode: 200, type: typeof(IEnumerable<Battle>), description: "")]
        [Route("battle/get_active_battles")]
        [HttpPost]
        public ActionResult GetActiveBattles()
        {
            try
            {
                var result =  _semifinalService.GetActiveBattles();
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                Logger.Log.LogError(ex.Message + ex.StackTrace);
                return Problem();
            }
        }


        /// <summary>
        /// Set or reset vote 
        /// </summary>
        /// <response code="200">Returns bool value</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">If something goes wrong on server</response>
        [SwaggerOperation("Get")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "True if success")]
        [Route("battle/vote")]
        [HttpPost]
        public ActionResult Vote(long battleId, long semifinalistId)
        {
            try
            {
                var userGuid = GetUserGuid();
                var result = _semifinalService.Vote(userGuid, battleId, semifinalistId);
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                Logger.Log.LogError(ex.Message + ex.StackTrace);
                return Problem();
            }
        }
    }
}
