Configurations and Services automatic registration
==================================================

This package provides a convenient method for automatically registering configurations and services in a DotNet application.

It proves beneficial when you're dealing with numerous configurations and services and prefer not to register them manually.

The package includes various attributes that facilitate automatic registration of configurations and services.

## Installation

To install this package, you can use the NuGet package manager or the dotnet CLI.

```bash
dotnet add package DiAutoRegistration
```

```bash
Install-Package DiAutoRegistration
```

Then, you need to call the ***AddDiAutoRegistration*** method on the Services in the Startup class or program.cs if you are using minimal API:

```csharp
    builder.Services.AddDiAutoRegistration(builder.Configuration);
```

## Available attributes for service registration

    ScopedServiceAttribute
    SingletonServiceAttribute
    TransientServiceAttribute

## Available attributes for configuration registration

    ConfigurationAttribute

## Configurations

To automatically register configurations:

**Add your configuration to the appsettings.json:**

```json
{
  "MyConfiguration": {
    "MyConfiguration1": "Value1",
    "MyConfiguration2": "Value2"
  }
}
```

**Create a class that contains the configuration properties:**

```csharp

public class MyConfiguration
{
    public string MyConfiguration1 { get; set; }
    public string MyConfiguration2 { get; set; }
}

```
   
**Add the ConfigurationAttribute to the class**

  ```csharp
  
[Configuration]
public class MyConfiguration
{
    public string MyConfiguration1 { get; set; }
    public string MyConfiguration2 { get; set; }
}
  
```

The configuration will be automatically registered and you can inject it:

```csharp

public class MyService
{
    private readonly MyConfiguration _myConfiguration;

    public MyService(IOption<MyConfiguration> myConfiguration)
    {
        var _myConfiguration = myConfiguration.Value;
    } 
    
    public string GetConfiguration1()
    {
        return _myConfiguration.MyConfiguration1;
    }
}

```
   
By default, the ConfigurationAttribute will use the class name to match the section name in the appsettings.json file. 

If you want to use a different name, you can pass it as a parameter to the attribute:

suppose you have the following section in the appsettings.json file:

```json

{
  "MySectionName": {
    "MyConfiguration1": "Value1",
    "MyConfiguration": "Value2"
  }
}

```

You need to pass "MySectionName" as a parameter to the ConfigurationAttribute:

```csharp

[Configuration("MySectionName")]
public class MyConfiguration
{
    public string MyConfiguration1 { get; set; }
    public string MyConfiguration2 { get; set; }
}

```

## Services

To automatically register services:

**Create a class that contains the service:**

```csharp

public class MyService: IMyService
{
    public void DoSomething()
    {
        Console.WriteLine("Doing something");
    }
}

```

**Add one of available attributes to the class. Choose the Attribute that matches the lifetime of the service**

```csharp

[ScopedService]
public class MyService: IMyService
{
    public void DoSomething()
    {
        Console.WriteLine("Doing something");
    }
}

```

**The service will be automatically registered and you can inject it**

```csharp

    [ScopedService]
    public class MyOtherService
    {
        private readonly IMyService _myService;
    
        public MyOtherService(IMyService myService)
        {
            _myService = myService;
        }
}

```

You can pass the interface that the service implements as a parameter to the attribute. 

The service will be only registered for that interface. This is the recommended way to register services.

```csharp

[ScopedService(typeof(IMyService2))]
public class MyService: IMyService, IMyService2
{
    public void DoSomething()
    {
        Console.WriteLine("Doing something");
    }
}

```

## How services are registered

The package will register services based on the following rules:

1. If you pass an interface to the attribute, the service will be registered for that interface.

2. If you don't pass any interface, the service will be registered for the interface with the same name as the class (prefixed with I).

3. If an interface with the same name as the class does not exist, the service will be registered for all interfaces it directly implements.

    This implies that all interfaces from the base type (if any) will be excluded.

    Furthermore, if the service directly implements an interface that the base class implements, that interface will also be excluded.

4. If no interface is implemented, the service will be registered with its type.


**The best way** to register services is to always:
    
- implement an interface with same name as the class and use the attribute without parameters: 
```csharp
        [ScopedService]
        class MyService: IMyService
```
- or pass the interface as a parameter to the attribute
```csharp
        [ScopedService(typeof(IMyService))]
        class MyService: IMyService
```


## License

This project is licensed under the MIT License - see the [LICENSE.txt](LICENSE.txt) file for details
