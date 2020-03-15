using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Avatar.App.Api.Handlers;
using Avatar.App.Api.Models;
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
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [Route("get")]
        [SwaggerResponse(statusCode: 200, type: typeof(ICollection<UserProfileModel>), description: "Rating Json")]
        [HttpGet]
        public async Task<ActionResult> Get(int number)
        {
            try
            {
                var userProfiles = await _ratingService.GetCommonRatingAsync(number);
                
                return new JsonResult(ConvertModelHandler.UserProfilesToUserProfileModels(userProfiles));
            }
            catch(Exception ex)
            {
                Logger.Log.LogError(ex.Message + ex.StackTrace);
                return Problem();
            }
        }

        

        #region Private Methods

        private Guid? GetUserGuid()
        {
            var nameIdentifier = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (nameIdentifier == null) return null;
            return Guid.Parse(nameIdentifier.Value);
        }

        #endregion
    }
}