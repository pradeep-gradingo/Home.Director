using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Home.Director.Controller.Model;
using Home.Director.Controller.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Home.Director.Controller.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigurationController : ControllerBase
    {
        
        private readonly ILogger<ConfigurationController> _logger;
        private readonly ConfigurationService _configurationService;

        public ConfigurationController(ILogger<ConfigurationController> logger, ConfigurationService configurationService)
        {
            _logger = logger;
            _configurationService = configurationService;
        }

        [HttpGet]
        public IActionResult GetConfiguration()
        {
            return Ok(_configurationService.Settings);
        }

        [HttpPost]
        public IActionResult SetConfiguration(Settings settings)
        {
            _configurationService.Settings = settings;
            return Ok(_configurationService.Settings);
        }

        [HttpPut("enabled/{value}")]
        public IActionResult SetEnabled(bool value)
        {
            _configurationService.Settings.Enabled = value;
            return Ok(_configurationService.Settings);
        }

        [HttpPut("FlipVertical/{value}")]
        public IActionResult SetFlipVertical(bool value)
        {
            _configurationService.Settings.FlipVertical = value;
            return Ok(_configurationService.Settings);
        }

        [HttpPut("FlipHorizontal/{value}")]
        public IActionResult SetFlipHorizontal(bool value)
        {
            _configurationService.Settings.FlipHorizontal = value;
            return Ok(_configurationService.Settings);
        }

        [HttpPut("Frequency/{value}")]
        public IActionResult SetFlipHorizontal(int value)
        {
            _configurationService.Settings.Frequency = value;
            return Ok(_configurationService.Settings);
        }

        [HttpPut("RawPath/{value}")]
        public IActionResult SetRawPath(string value)
        {
            var path = HttpUtility.UrlDecode(value);
            _configurationService.Settings.RawPath = path;
            return Ok(_configurationService.Settings);
        }

        [HttpPut("ReadyToUploadPath/{value}")]
        public IActionResult SetReadyToUploadPath(string value)
        {
            var path = HttpUtility.UrlDecode(value);
            _configurationService.Settings.ReadyToUploadPath = path;
            return Ok(_configurationService.Settings);
        }

        [HttpPut("ArchivePath/{value}")]
        public IActionResult SetArchivePath(string value)
        {
            var path = HttpUtility.UrlDecode(value);
            _configurationService.Settings.ArchivePath = path;
            return Ok(_configurationService.Settings);
        }
    }
}
