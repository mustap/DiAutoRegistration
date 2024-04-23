using DiAutoRegistration.Attributes;

namespace DiAutoRegistrationTests.Services;

public interface IScopedServiceWithAttributeWithNoParams
{
    public string GetName();
}

[ScopedService]
public class ScopedServiceWithAttributeWithNoParams : IScopedServiceWithAttributeWithNoParams
{
    public string GetName() => "IScopedServiceWithAttributeWithNoParams";
}


public interface IScopedServiceWithAttributeWithParams
{
    public string GetName();
}

[ScopedService(typeof(IScopedServiceWithAttributeWithParams))]
public class ScopedServiceWithAttributeWithParams : IScopedServiceWithAttributeWithParams
{
    public string GetName() => "IScopedServiceWithAttributeWithParams";
}

[ScopedService]
public class ScopedServiceWithAttributeWithNoParamsAndNoInterface
{
    public string GetName() => "ScopedServiceWithAttributeWithNoParamsAndNoInterface";
}


public interface IScopedServiceInterface1
{
    public string GetName();
}
public interface IScopedServiceInterface2
{
    public int GetAge();
}
[ScopedService]
public class ScopedServiceWithAttributeWithNoParamsAndWithMultipleInterfaces : IScopedServiceInterface1, IScopedServiceInterface2
{
    public string GetName() => "ScopedServiceWithAttributeWithNoParamsAndWithMultipleInterfaces";
    public int GetAge() => 20;
}


[ScopedService]
public class ScopedServiceWithAttributeWithNoParamsAndWithBaseTypeAndWithNoInterface : ScopedServiceWithAttributeWithNoParamsAndWithMultipleInterfaces
{
    public string GetString() => "ScopedServiceWithAttributeWithNoParamsAndWithBaseTypeAndWithNoInterface";
}


public interface IMyScopedInterface
{
    public string GetString();
}
[ScopedService]
public class ScopedServiceWithAttributeWithNoParamsAndWithBaseTypeAndWithInterface : ScopedServiceWithAttributeWithNoParamsAndWithMultipleInterfaces, IMyScopedInterface
{
    public string GetString() => "ScopedServiceWithAttributeWithNoParamsAndWithBaseTypeAndWithNoInterface";
}

// tests: dependency injected services

public interface IScopedServiceDependencyInjected
{
    public string GetName();
}

[ScopedService]
public class ScopedServiceDependencyInjected: IScopedServiceDependencyInjected
{
    public string GetName() => "DependencyInjectedService";
}


public interface IScopedServiceWithDi
{
    public string GetName();
}

[ScopedService]
public class ScopedServiceWithDi(IScopedServiceDependencyInjected service): IScopedServiceWithDi
{
    public string GetName() => service.GetName();
}
