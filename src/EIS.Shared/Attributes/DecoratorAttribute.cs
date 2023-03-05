namespace EIS.Shared.Attributes;

// Marker attribute - avoid circular DI for the decorated types
[AttributeUsage(AttributeTargets.Class)]
public sealed class DecoratorAttribute : Attribute
{
}