using System;
using Home.Director.Controller.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Home.Director.Controller.Services
{
    public class ConfigurationService
    {
        private readonly ILogger<ConfigurationService> _logger;
        public Settings Settings { get; set; }
        public ConfigurationService(ILogger<ConfigurationService> logger, IOptionsMonitor<Settings> optionsSnapshot)
        {
            _logger = logger;
            Settings = optionsSnapshot.CurrentValue;
            _logger.LogInformation("Initialized configuration service...");
        }

    }
}
