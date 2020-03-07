using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Api.Models;
using Avatar.App.Entities;
using Avatar.App.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        [SwaggerResponse(statusCode: 200, type: typeof(ICollection<RatingItemModel>), description: "Rating Json")]
        [HttpGet]
        public async Task<ActionResult> Get(int number)
        {
            try
            {
                var ratingItems = await _ratingService.GetAsync(number);
                var ratingItemModels = (from ratingItem in ratingItems
                    let videoModels = ratingItem.User.LoadedVideos.Select(v => new VideoModel
                    {
                        Name = v.Name,
                        StartTime = v.StartTime,
                        EndTime = v.EndTime,
                        IsActive = v.IsActive
                    }).ToList()
                    select new RatingItemModel
                    {
                        LikesNumber = ratingItem.LikesNumber,
                        User = new UserModel
                        {
                            Name = ratingItem.User.Name,
                            Description = ratingItem.User.Description,
                            Guid = ratingItem.User.Guid.ToString(),
                            Videos = videoModels
                        }
                    }).ToList();
                return new JsonResult(ratingItemModels);
            }
            catch(Exception ex)
            {
                Logger.Log.LogError(ex.Message + ex.StackTrace);
                return Problem();
            }
        }
    }
}