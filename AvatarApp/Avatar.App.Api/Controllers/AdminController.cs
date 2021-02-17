using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Administration;
using Avatar.App.Administration.Models;
using Avatar.App.Api.Models.Administration;
using Avatar.App.Authentication;
using Avatar.App.Semifinal;
using Avatar.App.Semifinal.CData;
using Avatar.App.SharedKernel.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Avatar.App.Api.Controllers
{
    [Route("api/admin")]
    [Authorize]
    [ApiController]
    public class AdminController : BaseAuthorizeController
    {
        private readonly IAdministrationComponent _administrationComponent;
        private readonly ISemifinalComponent _semifinalComponent;
        private readonly AvatarAppSettings _avatarAppSettings;

        public AdminController(IAdministrationComponent administrationComponent, IOptions<AvatarAppSettings> avatarAppOptions, ISemifinalComponent semifinalComponent, IAuthenticationComponent authenticationComponent): base(authenticationComponent)
        {
            _administrationComponent = administrationComponent;
            _semifinalComponent = semifinalComponent;
            _avatarAppSettings = avatarAppOptions.Value;
        }

        [Route("get_videos")]
        [HttpGet]
        public async Task<IEnumerable<ContestantModerationView>> GetUncheckedVideoList(int number)
        {

            if (!CheckAdminRight()) return new List<ContestantModerationView>();
            var uncheckedVideos = await _administrationComponent.GetUncheckedVideosAsync(number);
            return uncheckedVideos.Select(video => new ContestantModerationView(video));
        }

        [Route("moderate")]
        [HttpGet]
        public async Task SetVideoStatus(string name, bool isApproved)
        {
            if (!CheckAdminRight()) return;
            await _administrationComponent.SetApproveStatusAsync(name, isApproved);
        }

        [Route("battle/create")]
        [HttpPost]
        public async Task CreateBattle(BattleCData battleCData)
        {
            if (!CheckAdminRight()) return;
            await _semifinalComponent.AddBattle(battleCData);
        }

        [Route("general_stat")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<GeneralStatistics> GetStatistics()
        {
            return await _administrationComponent.GetGeneralStatistics();
        }

        private bool CheckAdminRight()
        {
            var userGuid = GetUserGuid();
            return userGuid == Guid.Parse(_avatarAppSettings.AdminGuid);
        }
    }
}