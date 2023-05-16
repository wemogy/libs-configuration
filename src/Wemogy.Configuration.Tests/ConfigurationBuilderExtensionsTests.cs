using System.Linq;
using Microsoft.Extensions.Configuration;
using Wemogy.Configuration;
using Xunit;

namespace Wemogy.Configuration.Tests;

public class ConfigurationBuilderExtensionsTests
{
    [Theory]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData("Development", "DevelopmentValue")]
    [InlineData("nonExistingEnvironment", "")]
    public void AddDefaultJsonFiles_IncludesEnvironmentJson(string environment, string testValue)
    {
        // Arrange
        var builder = new ConfigurationBuilder();

        // Act
        builder.AddDefaultJsonFiles(environment);
        var config = builder.Build();

        // Assert
        Assert.NotEmpty(builder.Sources);
        Assert.Equal(testValue, config["TestValue"]);
    }
}
