using DiAutoRegistration.Extensions;
using Microsoft.Extensions.DependencyInjection;
using DiAutoRegistrationTests.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace DiAutoRegistrationTests;

public class SingletonServicesTest
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
    public void TestSingletonServiceWithAttributeWithNoParamsAndWithInterfaceWithTheSameNameShouldOnlyBeRegisteredByThatInterface()
    {
        // Arrange


        // Act
        var singletonService = provider.GetRequiredService<ISingletonServiceWithAttributeWithNoParams>();
        var descriptors = services.Where(s => s.ImplementationType == typeof(SingletonServiceWithAttributeWithNoParams)).ToList();

        // Assert
        Assert.NotNull(singletonService);

        // Only one service is registered
        Assert.That(descriptors.Count, Is.EqualTo(1) );

        // and is registered as singleton
        Assert.That(descriptors.FirstOrDefault()?.Lifetime, Is.EqualTo(ServiceLifetime.Singleton));
    }

    [Test]
    public void TestSingletonServiceWithAttributeWithParamsShouldOnlyBeRegisteredWithTheInterfaceFromThatParam()
    {
        // Arrange


        // Act
        var singletonService = provider.GetRequiredService<ISingletonServiceWithAttributeWithParams>();
        var descriptors = services.Where(s => s.ImplementationType == typeof(SingletonServiceWithAttributeWithParams)).ToList();

        // Assert
        Assert.NotNull(singletonService);

        // Only one service is registered
        Assert.That(descriptors.Count, Is.EqualTo(1) );

        // and is registered as singleton
        Assert.That(descriptors.FirstOrDefault()?.Lifetime, Is.EqualTo(ServiceLifetime.Singleton));
    }


    [Test]
    public void TestSingletonServiceWithAttributeWithNoParamsAndNoInterfaceShouldOnlyBeRegisteredWithItsType()
    {
        // Arrange

        // Act
        var singletonService = provider.GetRequiredService<SingletonServiceWithAttributeWithNoParamsAndNoInterface>();
        var descriptors = services.Where(s => s.ImplementationType == typeof(SingletonServiceWithAttributeWithNoParamsAndNoInterface)).ToList();

        // Assert
        Assert.NotNull(singletonService);

        // Only one service is registered
        Assert.That(descriptors.Count, Is.EqualTo(1) );

        // and is registered as singleton
        Assert.That(descriptors.FirstOrDefault(s => s.ImplementationType == typeof(SingletonServiceWithAttributeWithNoParamsAndNoInterface))?.Lifetime, Is.EqualTo(ServiceLifetime.Singleton));
    }

    [Test]
    public void TestSingletonServiceWithAttributeWithNoParamsAndMultipleInterfaceShouldBeRegisteredWithAllProvidedInterfaces()
    {
        // Arrange


        // Act: We have a service that implements 2 interfaces: ISingletonServiceInterface1 and ISingletonServiceInterface2
        var singletonService1 = provider.GetRequiredService<ISingletonServiceInterface1>();
        var singletonService2 = provider.GetRequiredService<ISingletonServiceInterface2>();
        var descriptors = services.Where(s => s.ImplementationType == typeof(SingletonServiceWithAttributeWithNoParamsAndWithMultipleInterfaces)).ToList();

        // Assert
        Assert.NotNull(singletonService1);
        Assert.NotNull(singletonService2);

        // Only one service implemented. The others have ImplementationFactory that return the same instance
        Assert.That(descriptors.Count, Is.EqualTo(1));

        // and are registered as singleton
        var count = services.Count(e => (e.ServiceType == typeof(ISingletonServiceInterface1) || e.ServiceType == typeof(ISingletonServiceInterface2)) && e.Lifetime == ServiceLifetime.Singleton);
        Assert.That(count, Is.EqualTo(2) );

        // and are the same instance
        Assert.That(singletonService2, Is.SameAs(singletonService1));
    }

    [Test]
    public void TestSingletonServiceWithAttributeWithNoParamsAndWithBaseTypeAndWithNoInterfaceShouldBeRegisteredWithItsType()
    {
        // Arrange

        // Act
        var singletonService = provider.GetRequiredService<SingletonServiceWithAttributeWithNoParamsAndWithBaseTypeAndWithNoInterface>();
        var descriptors = services.Where(s => s.ImplementationType == typeof(SingletonServiceWithAttributeWithNoParamsAndWithBaseTypeAndWithNoInterface)).ToList();

        // Assert
        // The service is registered
        Assert.NotNull(singletonService);

        // Only one service is registered
        Assert.That(descriptors.Count, Is.EqualTo(1) );

        // and is registered as singleton
        Assert.That(descriptors.All(e => e.Lifetime == ServiceLifetime.Singleton), Is.True);
    }


    [Test]
    public void TestSingletonServiceWithAttributeWithNoParamsAndWithBaseTypeAndWithInterfaceShouldOnlyBeRegisteredWithItsIntarface()
    {
        // Arrange

        // Act
        var singletonService = provider.GetRequiredService<IMySingletonInterface>();
        var descriptors = services.Where(s => s.ImplementationType == typeof(SingletonServiceWithAttributeWithNoParamsAndWithBaseTypeAndWithInterface)).ToList();

        // Assert
        Assert.NotNull(singletonService);

        // Only one service is registered
        Assert.That(descriptors.Count, Is.EqualTo(1) );

        // and is registered as singleton
        Assert.That(descriptors.All(e => e.Lifetime == ServiceLifetime.Singleton), Is.True);
    }

    [Test]
    public void TestSingletonServiceWithDependencyInjectedShouldGetTheDiService()
    {
        // Arrange

        // Act
        var singletonService = provider.GetRequiredService<ISingletonServiceWithDi>();

        // Assert
        Assert.NotNull(singletonService);

        // The service should get the injected service of type ISingletonServiceDependencyInjected
        Assert.That(singletonService.GetName(), Is.EqualTo("DependencyInjectedService"));
    }



}