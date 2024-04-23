using DiAutoRegistration.Attributes;

namespace DiAutoRegistrationTests.Services;

public interface ISingletonServiceWithAttributeWithNoParams
{
    public string GetName();
}

[SingletonService]
public class SingletonServiceWithAttributeWithNoParams : ISingletonServiceWithAttributeWithNoParams
{
    public string GetName() => "ISingletonServiceWithAttributeWithNoParams";
}


public interface ISingletonServiceWithAttributeWithParams
{
    public string GetName();
}

[SingletonService(typeof(ISingletonServiceWithAttributeWithParams))]
public class SingletonServiceWithAttributeWithParams : ISingletonServiceWithAttributeWithParams
{
    public string GetName() => "ISingletonServiceWithAttributeWithParams";
}

[SingletonService]
public class SingletonServiceWithAttributeWithNoParamsAndNoInterface
{
    public string GetName() => "SingletonServiceWithAttributeWithNoParamsAndNoInterface";
}


public interface ISingletonServiceInterface1
{
    public string GetName();
}
public interface ISingletonServiceInterface2
{
    public int GetAge();
}
[SingletonService]
public class SingletonServiceWithAttributeWithNoParamsAndWithMultipleInterfaces : ISingletonServiceInterface1, ISingletonServiceInterface2
{
    public string GetName() => "SingletonServiceWithAttributeWithNoParamsAndWithMultipleInterfaces";
    public int GetAge() => 20;
}


[SingletonService]
public class SingletonServiceWithAttributeWithNoParamsAndWithBaseTypeAndWithNoInterface : SingletonServiceWithAttributeWithNoParamsAndWithMultipleInterfaces
{
    public string GetString() => "SingletonServiceWithAttributeWithNoParamsAndWithBaseTypeAndWithNoInterface";
}


public interface IMySingletonInterface
{
    public string GetString();
}
[SingletonService]
public class SingletonServiceWithAttributeWithNoParamsAndWithBaseTypeAndWithInterface : SingletonServiceWithAttributeWithNoParamsAndWithMultipleInterfaces, IMySingletonInterface
{
    public string GetString() => "SingletonServiceWithAttributeWithNoParamsAndWithBaseTypeAndWithNoInterface";
}

// tests: dependency injected services

public interface ISingletonServiceDependencyInjected
{
    public string GetName();
}

[SingletonService]
public class SingletonServiceDependencyInjected: ISingletonServiceDependencyInjected
{
    public string GetName() => "DependencyInjectedService";
}


public interface ISingletonServiceWithDi
{
    public string GetName();
}

[SingletonService]
public class SingletonServiceWithDi(ISingletonServiceDependencyInjected service): ISingletonServiceWithDi
{
    public string GetName() => service.GetName();
}
