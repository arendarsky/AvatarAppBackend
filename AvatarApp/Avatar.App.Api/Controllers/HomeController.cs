﻿using Microsoft.AspNetCore.Mvc;

namespace Avatar.App.Api.Controllers
{
    [Route("/")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
