using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Home.Director.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly ILogger<ConfigController> _logger;
        private readonly IOptionsSnapshot<Settings> _optionsSettings;
        public ConfigController(ILogger<ConfigController> logger, IOptionsSnapshot<Settings> optionsSettings)
        {
            _logger = logger;
            _optionsSettings = optionsSettings;
        }

        [HttpGet]
        public ActionResult GetCurrentConfig()
        {
            return Ok(_optionsSettings.Value);
        }
    }
}
