using System;
using System.Linq;
using System.Reflection;
using Dapr.Client;
using Dapr.Extensions.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Wemogy.Configuration
{
    public static class ConfigurationBuilderExtensions
    {
        /// Adds appsettings.json and appsettings.{environment}.json (optional) to the configuration
        /// </summary>
        /// <param name="builder">IConfigurationBuilder to use</param>
        /// <param name="context">HostBuilderContext for ASP.NET applications. Will be used for appsettings.{environment}.json if not empty</param>
        /// <returns>Updated IConfigurationBuilder</returns>
        public static IConfigurationBuilder AddDefaultJsonFiles(this IConfigurationBuilder builder, HostBuilderContext context)
        {
            return builder.AddDefaultJsonFiles(context.HostingEnvironment.EnvironmentName);
        }

        /// Adds appsettings.json and appsettings.{environment}.json (optional) to the configuration
        /// </summary>
        /// <param name="builder">IConfigurationBuilder to use</param>
        /// <param name="environment">Name of the environment. Will be used for appsettings.{environment}.json if not empty</param>
        /// <returns>Updated IConfigurationBuilder</returns>
        public static IConfigurationBuilder AddDefaultJsonFiles(this IConfigurationBuilder builder, string? environment = null)
        {
            builder.AddJsonFile("appsettings.json");

            if (!string.IsNullOrEmpty(environment))
            {
                builder.AddJsonFile($"appsettings.{environment}.json", optional: true);
            }

            return builder;
        }

        /// <summary>
        /// Adds a Dapr Secret Store to the configuration
        /// </summary>
        /// <param name="builder">The Configuration Builder.</param>
        /// <param name="daprClient">The Dapr client.</param>
        /// <param name="secretStore">Name of the Dapr Secret Store to use.</param>
        /// <param name="configurationSection">Section in the existing Configuration that contains an array of Secret Names</param>
        /// <returns></returns>
        public static IConfigurationBuilder AddDaprSecretStore(this IConfigurationBuilder builder, DaprClient daprClient, string secretStore = "", string configurationSection = "Secrets")
        {
            // Get Dapr Values from Configuration
            var configuration = builder.Build();

            if (string.IsNullOrEmpty(secretStore))
            {
                secretStore = configuration["SecretStore"];
            }

            Console.WriteLine($"Connecting to Dapr Secret Store '{secretStore}'...");

            var secretsSection = configuration.GetSection(configurationSection);
            if (!secretsSection.Exists())
            {
                Console.WriteLine($"WARNING: Configuration section '{configurationSection}' does not exist or is empty. Skipping adding Dapr Secret Store...");
                return builder;
            }

            if (Assembly.GetEntryAssembly()?.FullName?.Contains("dotnet-swagger") == true)
            {
                Console.WriteLine($"WARNING: dotnet-swagger detected. Skipping adding Dapr Secret Store...");
                return builder;
            }

            var secrets = secretsSection.Get<string[]>();
            foreach (var secret in secrets)
            {
                Console.WriteLine($"Trying to fetch secret '{secret}' from Dapr Secret Store '{secretStore}'...");
            }

            var secretDescriptors = secrets.Select(x => new DaprSecretDescriptor(x));

            builder.AddDaprSecretStore(secretStore, secretDescriptors, daprClient);
            return builder;
        }
    }
}
