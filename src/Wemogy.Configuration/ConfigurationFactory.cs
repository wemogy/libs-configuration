using System.Reflection;
using Dapr.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Wemogy.Configuration
{
    public static class ConfigurationFactory
    {
        /// <summary>
        /// Adds JSON files and Environment variables to the configuration and builds it in the following order:
        /// 1. appsettings.json
        /// 2. appsettings.{ENVIRONMENT}.json
        /// 3. Local User Secrets
        /// 4. Environment Variables
        /// </summary>
        /// <param name="environment">Name of the current environment (e.g. Development)</param>
        /// <returns>Built configuration</returns>
        public static IConfiguration BuildConfiguration(HostBuilderContext hostingContext)
        {
            return new ConfigurationBuilder()
                .AddDefaultJsonFiles(hostingContext)
                .AddUserSecrets(Assembly.GetCallingAssembly(), true)
                .AddEnvironmentVariables()
                .Build();
        }

        /// <summary>
        /// Adds JSON files and Environment variables to the configuration and builds it in the following order:
        /// 1. appsettings.json
        /// 2. appsettings.{ENVIRONMENT}.json
        /// 3. Local User Secrets
        /// 4. Environment Variables
        /// </summary>
        /// <returns>Built configuration</returns>
        public static IConfiguration BuildConfiguration(string environmentName = null)
        {
            return new ConfigurationBuilder()
                .AddDefaultJsonFiles(environmentName)
                .AddUserSecrets(Assembly.GetCallingAssembly(), true)
                .AddEnvironmentVariables()
                .Build();
        }

        /// <summary>
        /// Adds JSON files, a Dapr Secret Store and Environment variables to the configuration and builds it in the following order:
        /// 1. appsettings.json
        /// 2. appsettings.{ENVIRONMENT}.json
        /// 3. Dapr Secret Store
        /// 4. Local User Secrets
        /// 5. Environment Variables
        /// </summary>
        /// <param name="environment">Name of the current environment (e.g. Development)</param>
        /// <returns>Built configuration</returns>
        public static IConfiguration BuildConfigurationWithDapr(DaprClient daprClient, string secretStore, HostBuilderContext hostingContext, string configurationSection = "Secrets")
        {
            return new ConfigurationBuilder()
                .AddDefaultJsonFiles(hostingContext)
                .AddDaprSecretStore(daprClient, secretStore, configurationSection)
                .AddUserSecrets(Assembly.GetCallingAssembly(), true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
