using System;

namespace DiAutoRegistration.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class TransientServiceAttribute(Type? @interface): Attribute
{
    public TransientServiceAttribute() : this(null)
    {
    }

    public Type? TypeOfService { get; } = @interface;
}