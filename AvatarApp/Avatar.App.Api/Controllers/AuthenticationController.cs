using System.Threading.Tasks;
using Avatar.App.Api.Models.Authorization;
using Avatar.App.Authentication;
using Avatar.App.Authentication.CData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Avatar.App.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : BaseAuthorizeController
    {
        public AuthenticationController(IAuthenticationComponent authenticationComponent) : base(
            authenticationComponent)
        {
        }

        [Route("register")]
        [HttpPost]
        public async Task<bool> Register(UserCData user)
        {
            return await AuthenticationComponent.TryRegister(user);
        }

        [Route("authorize")]
        [HttpPost]
        public async Task<AuthorizationResponse> GetAuthorizationToken(string email, string password)
        {
            var jwtUser = await AuthenticationComponent.GetAuthorizationToken(email, password);
            return new AuthorizationResponse
            {
                ConfirmationRequired = jwtUser.IsUserExists && !jwtUser.IsEmailConfirmed,
                Token = jwtUser.JwtToken
            };
        }

        [Route("send")]
        [HttpGet]
        public async Task<bool> SendEmail(string email)
        {
            var user = await GetUser(email);

            if (user == null)
            {
                return false;
            }

            return await AuthenticationComponent.TrySendEmailConfirmation(user);
        }

        [Route("confirm")]
        [HttpPost]
        public async Task<bool> ConfirmEmail(string guid, string confirmCode)
        {
            var user = await GetUserByGuid(guid);

            if (user == null)
            {
                return false;
            }

            return await AuthenticationComponent.TryConfirmEmail(user, confirmCode);
        }

        [Route("send_reset")]
        [HttpGet]
        public async Task<bool> SendPasswordReset(string email)
        {
            var user = await GetUser(email);

            if (user == null)
            {
                return false;
            }

            return await AuthenticationComponent.TrySendPasswordReset(user);
        }

        [Route("password_change")]
        [HttpPost]
        public async Task<bool> ChangePassword(string guid, string confirmCode, string password)
        {
            var user = await GetUserByGuid(guid);

            if (user == null)
            {
                return false;
            }

            return await AuthenticationComponent.TryResetPassword(user, confirmCode, password);
        }

        [Authorize]
        [Route("firebase_set")]
        [HttpPost]
        public async Task SetFireBaseId([FromBody] string fireBaseId)
        {
            var user = await GetUser();

            if (user == null)
            {
                return;
            }

            await AuthenticationComponent.SetFireBaseId(user, fireBaseId);
        }
    }
}
