using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Avatar.App.Api.Handlers;
using Avatar.App.Api.Models;
using Avatar.App.Api.Models.UserModels;
using Avatar.App.Core.Exceptions;
using Avatar.App.Core.Services;
using Avatar.App.SharedKernel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace Avatar.App.Api.Controllers
{
    [Authorize]
    [Route("api/rating")]
    [ApiController]
    public class RatingController : BaseAuthorizeController
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [Route("get")]
        [SwaggerResponse(statusCode: 200, type: typeof(ICollection<RatingUserModel>), description: "Rating Json")]
        [HttpGet]
        public async Task<ActionResult> Get(int number)
        {
            try
            {
                var userProfiles = await _ratingService.GetCommonRatingAsync(number);
                
                return new JsonResult(ConvertModelHandler.UserProfilesToRatingUserModels(userProfiles));
            }
            catch(Exception ex)
            {
                Logger.Log.LogError(ex.Message + ex.StackTrace);
                return Problem();
            }
        }

        [Route("user/get")]
        [SwaggerResponse(statusCode: 200, type: typeof(int), description: "Rating Json")]
        [HttpGet]
        public async Task<ActionResult> GetUserRating()
        {
            try
            {
                var userGuid = GetUserGuid();

                var likesNumber = await _ratingService.GetUserRating(userGuid);

                return new JsonResult(likesNumber);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                Logger.Log.LogError(ex.Message + ex.StackTrace);
                return Problem();
            }
        }



        #region Private Methods

        #endregion
    }
}