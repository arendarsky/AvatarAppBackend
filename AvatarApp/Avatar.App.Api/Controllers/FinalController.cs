using System.Threading.Tasks;
using Avatar.App.Api.Models.Final;
using Avatar.App.Authentication;
using Avatar.App.Final;
using Avatar.App.Final.CreationData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Avatar.App.Api.Controllers
{
    [Authorize]
    [Route("api/final")]
    [ApiController]
    public class FinalController : BaseAuthorizeController
    {
        private readonly IFinalComponent _finalComponent;

        public FinalController(IFinalComponent finalComponent, IAuthenticationComponent authenticationComponent) : base(authenticationComponent)
        {
            _finalComponent = finalComponent;
        }

        [Route("vote")]
        [HttpPost]
        public async Task<bool> VoteTo([FromBody] long finalistId)
        {
            var user = await GetUser();
            var voteCreation = new FinalVoteCreation(user.Id, finalistId);
            return await _finalComponent.VoteToAsync(voteCreation);
        }

        [Route("get")]
        [HttpGet]
        public async Task<FinalView> GetFinal()
        {
            var user = await GetUser();
            var final = await _finalComponent.GetFinalAsync();
            return final != null ? new FinalView(final, user.Id) : null;
        }
    }
}
