using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Api.Handlers;
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

        /// <summary>
        /// Get overall rating
        /// </summary>
        /// <param name="number"></param>
        /// <response code="200">Returns overall rating</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">If something goes wrong on server</response>
        [SwaggerOperation("Get")]
        [Route("get")]
        [SwaggerResponse(statusCode: 200, type: typeof(ICollection<RatingUserModel>), description: "Rating Json")]
        [HttpGet]
        public ActionResult Get(int number)
        {
            try
            {
                var userProfiles = _ratingService.GetCommonRating(number);
                
                return new JsonResult(ConvertModelHandler.UserProfilesToRatingUserModels(userProfiles));
            }
            catch(Exception ex)
            {
                Logger.Log.LogError(ex.Message + ex.StackTrace);
                return Problem();
            }
        }

        /// <summary>
        /// Get personal rating
        /// </summary>
        /// <response code="200">Returns personal rating</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">If something goes wrong on server</response>
        [SwaggerOperation("GetUserRating")]
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

        /// <summary>
        /// Get semifinalists
        /// </summary>
        /// <response code="200">Returns semifinalists</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">If something goes wrong on server</response>
        [SwaggerOperation("GetSemifinalists")]
        [Route("get_semifinalists")]
        [SwaggerResponse(statusCode: 200, type: typeof(int), description: "Rating Json")]
        [HttpGet]
        public ActionResult GetSemifinalists()
        {
            try
            {
                var users = _ratingService.GetSemifinalists();
                var usersWithLikes = _ratingService.GetUsersWithLikesNumberAsync(users).ToList();
                return new JsonResult(ConvertModelHandler.UserPorfilesToSemifinalistUserModels(usersWithLikes));
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