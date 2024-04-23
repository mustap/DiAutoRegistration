using System;

namespace DiAutoRegistration.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class ConfigurationAttribute(string? sectionName) : Attribute
{
    public ConfigurationAttribute() : this(null)
    {
    }

    public string? SectionName { get; } = sectionName;
}