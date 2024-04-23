using DiAutoRegistration.Extensions;
using Microsoft.Extensions.DependencyInjection;
using DiAutoRegistrationTests.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace DiAutoRegistrationTests;

public class TransientServicesTest
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
    public void TestTransientServiceWithAttributeWithNoParamsAndWithInterfaceWithTheSameNameShouldOnlyBeRegisteredByThatInterface()
    {
        // Arrange


        // Act
        var transientService = provider.GetRequiredService<ITransientServiceWithAttributeWithNoParams>();
        var descriptors = services.Where(s => s.ImplementationType == typeof(TransientServiceWithAttributeWithNoParams)).ToList();

        // Assert
        Assert.NotNull(transientService);

        // Only one service is registered
        Assert.That(descriptors.Count, Is.EqualTo(1) );

        // and is registered as transient
        Assert.That(descriptors.FirstOrDefault()?.Lifetime, Is.EqualTo(ServiceLifetime.Transient));
    }

    [Test]
    public void TestTransientServiceWithAttributeWithParamsShouldOnlyBeRegisteredWithTheInterfaceFromThatParam()
    {
        // Arrange


        // Act
        var transientService = provider.GetRequiredService<ITransientServiceWithAttributeWithParams>();
        var descriptors = services.Where(s => s.ImplementationType == typeof(TransientServiceWithAttributeWithParams)).ToList();

        // Assert
        Assert.NotNull(transientService);

        // Only one service is registered
        Assert.That(descriptors.Count, Is.EqualTo(1) );

        // and is registered as transient
        Assert.That(descriptors.FirstOrDefault()?.Lifetime, Is.EqualTo(ServiceLifetime.Transient));
    }


    [Test]
    public void TestTransientServiceWithAttributeWithNoParamsAndNoInterfaceShouldOnlyBeRegisteredWithItsType()
    {
        // Arrange

        // Act
        var transientService = provider.GetRequiredService<TransientServiceWithAttributeWithNoParamsAndNoInterface>();
        var descriptors = services.Where(s => s.ImplementationType == typeof(TransientServiceWithAttributeWithNoParamsAndNoInterface)).ToList();

        // Assert
        Assert.NotNull(transientService);

        // Only one service is registered
        Assert.That(descriptors.Count, Is.EqualTo(1) );

        // and is registered as transient
        Assert.That(descriptors.FirstOrDefault(s => s.ImplementationType == typeof(TransientServiceWithAttributeWithNoParamsAndNoInterface))?.Lifetime, Is.EqualTo(ServiceLifetime.Transient));
    }

    [Test]
    public void TestTransientServiceWithAttributeWithNoParamsAndMultipleInterfaceShouldBeRegisteredWithAllProvidedInterfaces()
    {
        // Arrange


        // Act: We have a service that implements 2 interfaces: ITransientServiceInterface1 and ITransientServiceInterface2
        var transientService1 = provider.GetRequiredService<ITransientServiceInterface1>();
        var transientService2 = provider.GetRequiredService<ITransientServiceInterface2>();
        var descriptors = services.Where(s => s.ImplementationType == typeof(TransientServiceWithAttributeWithNoParamsAndWithMultipleInterfaces)).ToList();

        // Assert
        Assert.NotNull(transientService1);
        Assert.NotNull(transientService2);

        // Two services are registered: That what transient means
        Assert.That(descriptors.Count, Is.EqualTo(2));

        // and are registered as transient
        var count = services.Count(e => (e.ServiceType == typeof(ITransientServiceInterface1) || e.ServiceType == typeof(ITransientServiceInterface2)) && e.Lifetime == ServiceLifetime.Transient);
        Assert.That(count, Is.EqualTo(2) );
    }

    [Test]
    public void TestTransientServiceWithAttributeWithNoParamsAndWithBaseTypeAndWithNoInterfaceShouldBeRegisteredWithItsType()
    {
        // Arrange

        // Act
        var transientService = provider.GetRequiredService<TransientServiceWithAttributeWithNoParamsAndWithBaseTypeAndWithNoInterface>();
        var descriptors = services.Where(s => s.ImplementationType == typeof(TransientServiceWithAttributeWithNoParamsAndWithBaseTypeAndWithNoInterface)).ToList();

        // Assert
        // The service is registered
        Assert.NotNull(transientService);

        // Only one service is registered
        Assert.That(descriptors.Count, Is.EqualTo(1) );

        // and is registered as transient
        Assert.That(descriptors.All(e => e.Lifetime == ServiceLifetime.Transient), Is.True);
    }


    [Test]
    public void TestTransientServiceWithAttributeWithNoParamsAndWithBaseTypeAndWithInterfaceShouldOnlyBeRegisteredWithItsIntarface()
    {
        // Arrange

        // Act
        var transientService = provider.GetRequiredService<IMyTransientInterface>();
        var descriptors = services.Where(s => s.ImplementationType == typeof(TransientServiceWithAttributeWithNoParamsAndWithBaseTypeAndWithInterface)).ToList();

        // Assert
        Assert.NotNull(transientService);

        // Only one service is registered
        Assert.That(descriptors.Count, Is.EqualTo(1) );

        // and is registered as transient
        Assert.That(descriptors.All(e => e.Lifetime == ServiceLifetime.Transient), Is.True);
    }

    [Test]
    public void TestTransientServiceWithDependencyInjectedShouldGetTheDiService()
    {
        // Arrange

        // Act
        var transientService = provider.GetRequiredService<ITransientServiceWithDi>();

        // Assert
        Assert.NotNull(transientService);

        // The service should get the injected service of type ITransientServiceDependencyInjected
        Assert.That(transientService.GetName(), Is.EqualTo("DependencyInjectedService"));
    }



}