using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Types
{

    public class AppDetailsEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var applicationAssembly = Assembly.GetEntryAssembly();

            var name = applicationAssembly.GetName().Name;
            var version = applicationAssembly.GetName().Version;
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var machineName = Environment.MachineName;
            var username = Environment.UserName;

            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("ApplicationVersion", version));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("EnvironmentName", environmentName));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("MachineName", machineName));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("EnvUserName", username));
        }
    }
}
