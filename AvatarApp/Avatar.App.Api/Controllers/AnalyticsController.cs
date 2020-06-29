using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Avatar.App.Api.Controllers
{
    [Route("api/analytics")]
    [ApiController]
    public class AnalyticsController: BaseAuthorizeController
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        /// <summary>
        /// Get Statistics
        /// </summary>
        [SwaggerOperation("SendGeneralMessage")]
        [Route("general_stat")]
        [HttpGet]
        public JsonResult GetStatistics()
        {
            return new JsonResult(_analyticsService.GetGeneralStatistics());
        }
    }
}
