using DiAutoRegistration.Extensions;
using Microsoft.Extensions.DependencyInjection;
using DiAutoRegistrationTests.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace DiAutoRegistrationTests;

public class ScopedServicesTest
{
    ServiceCollection services;
    ServiceProvider provider;

    [SetUp]
    public void Setup()
    {
        services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        services.AddDiAutoRegistration(configuration);
        provider = services.BuildServiceProvider();
    }

    // tear down
    [TearDown]
    public void TearDown()
    {
        services = null;
        provider = null;
    }


    [Test]
    public void TestScopedServiceWithAttributeWithNoParamsAndWithInterfaceWithTheSameNameShouldOnlyBeRegisteredByThatInterface()
    {
        // Arrange


        // Act
        var scopedService = provider.GetRequiredService<IScopedServiceWithAttributeWithNoParams>();
        var descriptors = services.Where(s => s.ImplementationType == typeof(ScopedServiceWithAttributeWithNoParams)).ToList();

        // Assert
        Assert.NotNull(scopedService);

        // Only one service is registered
        Assert.That(descriptors.Count, Is.EqualTo(1) );

        // and is registered as scoped
        Assert.That(descriptors.FirstOrDefault()?.Lifetime, Is.EqualTo(ServiceLifetime.Scoped));
    }

    [Test]
    public void TestScopedServiceWithAttributeWithParamsShouldOnlyBeRegisteredWithTheInterfaceFromThatParam()
    {
        // Arrange


        // Act
        var scopedService = provider.GetRequiredService<IScopedServiceWithAttributeWithParams>();
        var descriptors = services.Where(s => s.ImplementationType == typeof(ScopedServiceWithAttributeWithParams)).ToList();

        // Assert
        Assert.NotNull(scopedService);

        // Only one service is registered
        Assert.That(descriptors.Count, Is.EqualTo(1) );

        // and is registered as scoped
        Assert.That(descriptors.FirstOrDefault()?.Lifetime, Is.EqualTo(ServiceLifetime.Scoped));
    }


    [Test]
    public void TestScopedServiceWithAttributeWithNoParamsAndNoInterfaceShouldOnlyBeRegisteredWithItsType()
    {
        // Arrange

        // Act
        var scopedService = provider.GetRequiredService<ScopedServiceWithAttributeWithNoParamsAndNoInterface>();
        var descriptors = services.Where(s => s.ImplementationType == typeof(ScopedServiceWithAttributeWithNoParamsAndNoInterface)).ToList();

        // Assert
        Assert.NotNull(scopedService);

        // Only one service is registered
        Assert.That(descriptors.Count, Is.EqualTo(1) );

        // and is registered as scoped
        Assert.That(descriptors.FirstOrDefault(s => s.ImplementationType == typeof(ScopedServiceWithAttributeWithNoParamsAndNoInterface))?.Lifetime, Is.EqualTo(ServiceLifetime.Scoped));
    }

    [Test]
    public void TestScopedServiceWithAttributeWithNoParamsAndMultipleInterfaceShouldBeRegisteredWithAllProvidedInterfaces()
    {
        // Arrange


        // Act: We have a service that implements 2 interfaces: IScopedServiceInterface1 and IScopedServiceInterface2
        var scopedService1 = provider.GetRequiredService<IScopedServiceInterface1>();
        var scopedService2 = provider.GetRequiredService<IScopedServiceInterface2>();
        var descriptors = services.Where(s => s.ImplementationType == typeof(ScopedServiceWithAttributeWithNoParamsAndWithMultipleInterfaces)).ToList();

        // Assert
        Assert.NotNull(scopedService1);
        Assert.NotNull(scopedService2);

        // Only one service implemented. The others have ImplementationFactory that return the same instance
        Assert.That(descriptors.Count, Is.EqualTo(1));

        // and are registered as scoped
        var count = services.Count(e => (e.ServiceType == typeof(IScopedServiceInterface1) || e.ServiceType == typeof(IScopedServiceInterface2)) && e.Lifetime == ServiceLifetime.Scoped);
        Assert.That(count, Is.EqualTo(2) );

        // and are the same instance
        Assert.That(scopedService2, Is.SameAs(scopedService1));
    }

    [Test]
    public void TestScopedServiceWithAttributeWithNoParamsAndWithBaseTypeAndWithNoInterfaceShouldBeRegisteredWithItsType()
    {
        // Arrange

        // Act
        var scopedService = provider.GetRequiredService<ScopedServiceWithAttributeWithNoParamsAndWithBaseTypeAndWithNoInterface>();
        var descriptors = services.Where(s => s.ImplementationType == typeof(ScopedServiceWithAttributeWithNoParamsAndWithBaseTypeAndWithNoInterface)).ToList();

        // Assert
        // The service is registered
        Assert.NotNull(scopedService);

        // Only one service is registered
        Assert.That(descriptors.Count, Is.EqualTo(1) );

        // and is registered as scoped
        Assert.That(descriptors.All(e => e.Lifetime == ServiceLifetime.Scoped), Is.True);
    }


    [Test]
    public void TestScopedServiceWithAttributeWithNoParamsAndWithBaseTypeAndWithInterfaceShouldOnlyBeRegisteredWithItsIntarface()
    {
        // Arrange

        // Act
        var scopedService = provider.GetRequiredService<IMyScopedInterface>();
        var descriptors = services.Where(s => s.ImplementationType == typeof(ScopedServiceWithAttributeWithNoParamsAndWithBaseTypeAndWithInterface)).ToList();

        // Assert
        Assert.NotNull(scopedService);

        // Only one service is registered
        Assert.That(descriptors.Count, Is.EqualTo(1) );

        // and is registered as scoped
        Assert.That(descriptors.All(e => e.Lifetime == ServiceLifetime.Scoped), Is.True);
    }

    [Test]
    public void TestScopedServiceWithDependencyInjectedShouldGetTheDiService()
    {
        // Arrange

        // Act
        var scopedService = provider.GetRequiredService<IScopedServiceWithDi>();

        // Assert
        Assert.NotNull(scopedService);

        // The service should get the injected service of type IScopedServiceDependencyInjected
        Assert.That(scopedService.GetName(), Is.EqualTo("DependencyInjectedService"));
    }



}