using Avatar.App.Final;
using Microsoft.AspNetCore.Mvc;

namespace Avatar.App.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinalController : ControllerBase
    {
        private readonly IFinalComponent _finalComponent;

        public FinalController(IFinalComponent finalComponent)
        {
            _finalComponent = finalComponent;
        }

        
    }
}
