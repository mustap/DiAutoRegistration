using System.Reflection;
using DiAutoRegistration.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace DiAutoRegistration.Extensions;

public static class AutomaticRegistrationExtension
{
    public static IServiceCollection AddDiAutoRegistration(this IServiceCollection services,
        IConfiguration configuration)
    {
        var types = GetAnnotatedTypes();

        RegisterScopedServices(services, types);
        RegisterTransientServices(services, types);
        RegisterSingletonServices(services, types);
        RegisterConfigurations(services, configuration, types);

        return services;
    }

    #region -- Private Helper Methods

    private static List<Type> GetAnnotatedTypes()
    {
        // Get the current assembly name
        var self = Assembly.GetExecutingAssembly().GetName().FullName;

        // Get only the assemblies that reference the current assembly
        var referringAssemblies = AppDomain
            .CurrentDomain.
            GetAssemblies() // Get all assemblies
            .Where(a => a.GetReferencedAssemblies().Select(i => i.FullName).Contains(self))
            .ToList();

        // Get only the types annotated with one of our attributes
        return referringAssemblies.SelectMany(a => a.GetExportedTypes())
            .Where(t => t.GetCustomAttribute<ConfigurationAttribute>() != null ||
                        t.GetCustomAttribute<ScopedServiceAttribute>() != null ||
                        t.GetCustomAttribute<TransientServiceAttribute>() != null ||
                        t.GetCustomAttribute<SingletonServiceAttribute>() != null)
            .ToList();
    }

    private static IServiceCollection RegisterConfigurations(IServiceCollection services, IConfiguration configuration,
        List<Type> types)
    {
        // Get all types with the ConfigurationAttribute
        var configurationTypes = types.Where(t => t.GetCustomAttribute<ConfigurationAttribute>() != null);

        // Get the generic Configure method
        var configure = typeof(OptionsConfigurationServiceCollectionExtensions)
            .GetMethod("Configure", new Type[] {typeof(IServiceCollection), typeof(IConfiguration)});

        foreach (var type in configurationTypes)
        {
            // Get the section name from the attribute, or use the type name
            var sectionName = type.GetCustomAttribute<ConfigurationAttribute>()?.SectionName ?? type.Name;
            var options = configuration.GetSection(sectionName);
            configure?.MakeGenericMethod(type).Invoke(null, new object[] {services, options});
        }

        return services;
    }

    private static IServiceCollection RegisterScopedServices(IServiceCollection services, List<Type> types)
    {
        var servicesTypes = types.Where(t => t.GetCustomAttribute<ScopedServiceAttribute>() != null);

        foreach (var type in servicesTypes)
        {
            var declaredInterface = type.GetCustomAttribute<ScopedServiceAttribute>()?.TypeOfService;
            var interfaces = GetDirectDeclaredInterfaces(type, declaredInterface);

            if (interfaces.Count == 0)
            {
                services.AddScoped(type);
                continue;
            }

            Type? firstInterface = null;

            foreach (var @interface in interfaces)
            {
                if(firstInterface != null)
                {
                    services.AddScoped(@interface, serviceProvider => serviceProvider.GetService(firstInterface));
                    continue;
                }
                services.AddScoped(@interface, type);
                firstInterface = @interface;
            }
        }

        return services;
    }
    private static IServiceCollection RegisterTransientServices(IServiceCollection services, List<Type> types)
    {
        var servicesTypes = types.Where(t => t.GetCustomAttribute<TransientServiceAttribute>() != null);

        foreach (var type in servicesTypes)
        {
            var declaredInterface = type.GetCustomAttribute<TransientServiceAttribute>()?.TypeOfService;
            var interfaces = GetDirectDeclaredInterfaces(type, declaredInterface);

            if (interfaces.Count == 0)
            {
                services.AddTransient(type);
                continue;
            }

            // We cannot register Transient services with the same instance
            // as we did with Scoped and Singleton services
            foreach (var @interface in interfaces)
            {
                services.AddTransient(@interface, type);
            }
        }

        return services;
    }

    private static IServiceCollection RegisterSingletonServices(IServiceCollection services, List<Type> types)
    {
        var servicesTypes = types.Where(t => t.GetCustomAttribute<SingletonServiceAttribute>() != null);

        foreach (var type in servicesTypes)
        {
            var declaredInterface = type.GetCustomAttribute<SingletonServiceAttribute>()?.TypeOfService;
            var interfaces = GetDirectDeclaredInterfaces(type, declaredInterface);

            if (interfaces.Count == 0)
            {
                services.AddSingleton(type);
                continue;
            }

            Type? firstInterface = null;
            foreach (var @interface in interfaces)
            {
                if(firstInterface != null)
                {
                    services.AddSingleton(@interface, serviceProvider => serviceProvider.GetService(firstInterface));
                    continue;
                }
                services.AddSingleton(@interface, type);
                firstInterface = @interface;
            }
        }

        return services;
    }

    private static List<Type> GetDirectDeclaredInterfaces(Type type, Type? declaredInterface)
    {
        // if we have an interface in the attribute, we use it
        if (declaredInterface != null)
        {
            return [declaredInterface];
        }

        // get all interfaces of the type
        var interfaces = type.GetInterfaces().ToList();

        // if the type has an interface with same name, we use it
        var interfaceType = interfaces.FirstOrDefault(i => i.Name == $"I{type.Name}");

        if(interfaceType != null)
        {
            return [interfaceType];
        }

        // if the type has no base type, we return the all interfaces
        if (type.BaseType == null)
        {
            return interfaces;
        }

        // if the type has a base type, we return the interfaces that are not in the base type.
        // There is a caveat here, if the type and the base type declare the same interface,
        // the interface it will ignored because of the Except method.
        // If you want to include the interfaces declared in the base type, you have to add it manually
        // in the Attribute. ie:
        // [TransientService(typeof(IMyInterface))]
        return interfaces.Except(type.BaseType.GetInterfaces()).ToList();
    }

    #endregion
}
