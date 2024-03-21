using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Repositories.Models;

namespace WebApplication1.Helpers
{
    public class AppSettings
    {
        public Logging Logging { get; set; }
        public string AllowedHosts { get; set; }
        public DependencyInjection DependencyInjection { get; set; }
        public DbConfig DatabaseConfiguration { get; set; }
        public string PasswordSecret { get; set; }
    }

    public class LogLevel
    {
        public string Default { get; set; }
        public string Microsoft { get; set; }

        [JsonProperty("Microsoft.Hosting.Lifetime")]
        public string MicrosoftHostingLifetime { get; set; }
    }

    public class Logging
    {
        public LogLevel LogLevel { get; set; }
    }

    public class DependencyInjection
    {
        public string ControllerAssembly { get; set; }
        public string ServiceAssembly { get; set; }
        public string RepositoryAssembly { get; set; }
        public string InterfaceAssembly { get; set; }
    }
}
