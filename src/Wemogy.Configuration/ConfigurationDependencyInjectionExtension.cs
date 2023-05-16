using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Wemogy.Configuration
{
    public static class ConfigurationDependencyInjectionExtension
    {
        public static IConfiguration AddConfiguration(this IServiceCollection serviceCollection)
        {
            var configuration = ConfigurationFactory.BuildConfiguration();
            serviceCollection.AddSingleton<IConfiguration>(configuration);
            return configuration;
        }
    }
}
