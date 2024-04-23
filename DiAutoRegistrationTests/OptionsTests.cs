using DiAutoRegistration.Extensions;
using DiAutoRegistrationTests.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DiAutoRegistrationTests;

public class OptionsTests
{
    ServiceProvider provider;
    WebApplicationBuilder builder = WebApplication.CreateBuilder();

    [SetUp]
    public void Setup()
    {
        builder.Services.AddDiAutoRegistration(builder.Configuration);
        provider = builder.Services.BuildServiceProvider();
    }

    [Test]
    public void TestConfigurationRegistrationWithClassNameAsSectionName()
    {
        // Arrange


        // Act
        builder.Services.AddDiAutoRegistration(builder.Configuration);

        // Assert
        var options = provider.GetRequiredService<IOptions<MyOptions>>();
        Assert.NotNull(options);
        Assert.That(options.Value.Name, Is.EqualTo("Jens"));
    }

    [Test]
    public void TestConfigurationRegistrationWithSectionNameParam()
    {
        // Arrange


        // Act
        builder.Services.AddDiAutoRegistration(builder.Configuration);

        // Assert
        var options = provider.GetRequiredService<IOptions<HerOptions>>();
        Assert.NotNull(options);
        Assert.That(options.Value.LastName, Is.EqualTo("Larsen"));
    }
}