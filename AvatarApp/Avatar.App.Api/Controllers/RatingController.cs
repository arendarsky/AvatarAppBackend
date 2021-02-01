using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Api.Handlers;
using Avatar.App.Api.Models.UserModels;
using Avatar.App.Core.Exceptions;
using Avatar.App.Core.Services;
using Avatar.App.Final;
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
        private readonly IFinalComponent _finalComponent;

        public RatingController(IRatingService ratingService, IFinalComponent finalComponent)
        {
            _ratingService = ratingService;
            _finalComponent = finalComponent;
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
        [HttpGet]
        public IEnumerable<RatingUserModel> Get(int number)
        {
            var userProfiles = _ratingService.GetCommonRating(number);
            return ConvertModelHandler.UserProfilesToRatingUserModels(userProfiles);
        }

        /// <summary>
        /// Get personal rating
        /// </summary>
        /// <response code="200">Returns personal rating</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">If something goes wrong on server</response>
        [SwaggerOperation("GetUserRating")]
        [Route("user/get")]
        [HttpGet]
        public async Task<int> GetUserRating()
        {
            var userGuid = GetUserGuid();
            var likesNumber = await _ratingService.GetUserRating(userGuid);
            return likesNumber;
        }

        /// <summary>
        /// Get semifinalists
        /// </summary>
        /// <response code="200">Returns semifinalists</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">If something goes wrong on server</response>
        [SwaggerOperation("GetSemifinalists")]
        [Route("get_semifinalists")]
        [HttpGet]
        public IEnumerable<SemifinalistUserModel> GetSemifinalists()
        {
            var semifinalistsProfiles = _ratingService.GetSemifinalists();
            return ConvertModelHandler.UserProfilesToSemifinalistUserModels(semifinalistsProfiles);
        }

        [HttpGet]
        [Route("get_finalists")]
        public async Task<IEnumerable<SemifinalistUserModel>> GetFinalists()
        {
            var finalists = await _finalComponent.GetFinalists();
            return ConvertModelHandler.UserProfilesToSemifinalistUserModels(finalists.ToList());
        }
    }
}