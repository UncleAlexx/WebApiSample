using EfCoreSample.Entities;
using FluentValidation;

namespace WebApplication3.Validators;
public sealed class ProviderValidator : AbstractValidator<Provider>
{
    public ProviderValidator()
    {
        RuleFor(provider => provider.Address).NotNull().NotEmpty().MinimumLength(15);
        RuleFor(provider => provider.FullName).NotNull().NotEmpty().MinimumLength(15);
        RuleFor(parts => parts.Phone).NotNull().Matches(EfCoreSample.Validation.ValidationPatterns.PhonePattern());
    }
}
