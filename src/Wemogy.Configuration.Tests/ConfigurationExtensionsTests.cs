using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Wemogy.Configuration.Tests;

public class ConfigurationExtensionsTests
{
    [Fact]
    public void GetRequiredValue_ThrowsException_WhenKeyIsMissing()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().Build();

        // Act
        void Act() => configuration.GetRequiredValue("MissingKey");

        // Assert
        Assert.Throws<InvalidOperationException>((Action)Act);
    }

    [Fact]
    public void GetRequiredValue_ReturnsValue_WhenKeyIsPresent()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string>("ExistingKey", "ExistingValue")
            })
            .Build();

        // Act
        var value = configuration.GetRequiredValue("ExistingKey");

        // Assert
        Assert.Equal("ExistingValue", value);
    }
}
