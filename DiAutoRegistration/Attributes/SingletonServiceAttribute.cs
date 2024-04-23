using System;

namespace DiAutoRegistration.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class SingletonServiceAttribute(Type? @interface): Attribute
{
    public SingletonServiceAttribute() : this(null)
    {
    }

    public Type? TypeOfService { get; } = @interface;
}