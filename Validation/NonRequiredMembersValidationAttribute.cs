namespace EfCoreSample.Validation;

[AttributeUsage(AttributeTargets.Property)]
public sealed class NonRequiredMembersValidationAttribute : Attribute
{
    public readonly ValidationType Type;

    public NonRequiredMembersValidationAttribute(ValidationType validationType) => Type = validationType;
}
