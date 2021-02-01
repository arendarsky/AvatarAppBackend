using System.Collections.Generic;
using System.Linq;
using Avatar.App.Api.Handlers;
using Avatar.App.Api.Models.UserModels;
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
