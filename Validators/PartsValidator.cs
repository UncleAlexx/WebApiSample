using EfCoreSample.Entities;
using FluentValidation;

namespace WebApplication3.Validators;
public sealed class PartsValidator : AbstractValidator<Parts>
{
    public PartsValidator()
    { 
        RuleFor(parts => parts.Count).GreaterThan(0);
        RuleFor(parts => parts.ProviderId).GreaterThan(0);
        RuleFor(parts => parts.Type).NotEmpty().NotNull().MinimumLength(10);
    }
}
