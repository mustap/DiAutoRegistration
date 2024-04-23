using DiAutoRegistration.Attributes;

namespace DiAutoRegistrationTests.Options;

[Configuration]
public class MyOptions
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; } = 0;
}

[Configuration("SecondOptions")]
public class HerOptions
{
    public string LastName { get; set; } = string.Empty;
    public int TheAge { get; set; } = 0;
}