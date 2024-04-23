using System;

namespace DiAutoRegistration.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class ScopedServiceAttribute(Type? @interface) : Attribute
{
    public ScopedServiceAttribute() : this(null)
    {
    }

    public Type? TypeOfService { get; } = @interface;
}