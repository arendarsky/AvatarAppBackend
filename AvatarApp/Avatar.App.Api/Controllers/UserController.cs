using Avatar.App.Service.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;



namespace Avatar.App.Api.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("set_name")]
        public async Task<IActionResult> SetName(string userName)
        {
            await _userService.SetName(userName);
            return Ok();
        }
    }
}
