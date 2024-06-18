using System;
using Microsoft.Extensions.Configuration;

namespace Wemogy.Configuration
{
    public static class ConfigurationExtensions
    {
        public static string GetRequiredValue(this IConfiguration configuration, string key)
        {
            var value = configuration[key];

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException($"Configuration value for key '{key}' is required.");
            }

            return value;
        }
    }
}
