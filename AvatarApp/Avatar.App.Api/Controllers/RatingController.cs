using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Api.Models.Rating;
using Avatar.App.Authentication;
using Avatar.App.Rating;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Avatar.App.Api.Controllers
{
    [Authorize]
    [Route("api/rating")]
    [ApiController]
    public class RatingController : BaseAuthorizeController
    {
        private readonly IRatingComponent _ratingComponent;

        public RatingController(IRatingComponent ratingComponent, IAuthenticationComponent authenticationComponent): base(authenticationComponent)
        {
            _ratingComponent = ratingComponent;
        }

        [Route("get")]
        [HttpGet]
        public async Task<IEnumerable<RatingContestantPerformanceView>> Get(int number)
        {
            var ratingContestants = await _ratingComponent.GetCommonRating(number);
            return ratingContestants.Select(contestant => new RatingContestantPerformanceView(contestant));
        }

        [Route("user/get")]
        [HttpGet]
        public async Task<int> GetUserRating()
        {
            var userGuid = GetUserGuid();
            return await _ratingComponent.GetUserRating(userGuid);
        }

        [Route("get_semifinalists")]
        [HttpGet]
        public async Task<IEnumerable<RatingContestantView>> GetSemifinalists()
        {
            var semifinalistsProfiles = await _ratingComponent.GetSemifinalists();
            return semifinalistsProfiles.Select(semifinalist => new RatingContestantView(semifinalist));
        }

        [HttpGet]
        [Route("get_finalists")]
        public async Task<IEnumerable<RatingContestantView>> GetFinalists()
        {
            var finalistsProfiles = await _ratingComponent.GetFinalists();
            return finalistsProfiles.Select(finalist => new RatingContestantView(finalist));
        }
    }
}