using DiAutoRegistration.Attributes;

namespace DiAutoRegistrationTests.Services;

public interface ITransientServiceWithAttributeWithNoParams
{
    public string GetName();
}

[TransientService]
public class TransientServiceWithAttributeWithNoParams : ITransientServiceWithAttributeWithNoParams
{
    public string GetName() => "ITransientServiceWithAttributeWithNoParams";
}


public interface ITransientServiceWithAttributeWithParams
{
    public string GetName();
}

[TransientService(typeof(ITransientServiceWithAttributeWithParams))]
public class TransientServiceWithAttributeWithParams : ITransientServiceWithAttributeWithParams
{
    public string GetName() => "ITransientServiceWithAttributeWithParams";
}

[TransientService]
public class TransientServiceWithAttributeWithNoParamsAndNoInterface
{
    public string GetName() => "TransientServiceWithAttributeWithNoParamsAndNoInterface";
}


public interface ITransientServiceInterface1
{
    public string GetName();
}
public interface ITransientServiceInterface2
{
    public int GetAge();
}
[TransientService]
public class TransientServiceWithAttributeWithNoParamsAndWithMultipleInterfaces : ITransientServiceInterface1, ITransientServiceInterface2
{
    public string GetName() => "TransientServiceWithAttributeWithNoParamsAndWithMultipleInterfaces";
    public int GetAge() => 20;
}


[TransientService]
public class TransientServiceWithAttributeWithNoParamsAndWithBaseTypeAndWithNoInterface : TransientServiceWithAttributeWithNoParamsAndWithMultipleInterfaces
{
    public string GetString() => "TransientServiceWithAttributeWithNoParamsAndWithBaseTypeAndWithNoInterface";
}


public interface IMyTransientInterface
{
    public string GetString();
}
[TransientService]
public class TransientServiceWithAttributeWithNoParamsAndWithBaseTypeAndWithInterface : TransientServiceWithAttributeWithNoParamsAndWithMultipleInterfaces, IMyTransientInterface
{
    public string GetString() => "TransientServiceWithAttributeWithNoParamsAndWithBaseTypeAndWithNoInterface";
}

// tests: dependency injected services

public interface ITransientServiceDependencyInjected
{
    public string GetName();
}

[TransientService]
public class TransientServiceDependencyInjected: ITransientServiceDependencyInjected
{
    public string GetName() => "DependencyInjectedService";
}


public interface ITransientServiceWithDi
{
    public string GetName();
}

[TransientService]
public class TransientServiceWithDi(ITransientServiceDependencyInjected service): ITransientServiceWithDi
{
    public string GetName() => service.GetName();
}
