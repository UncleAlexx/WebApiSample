using EfCoreSample.Entities;
using EfCoreSample.Validation;
using FluentValidation;

namespace WebApplication3.Validators;
public sealed class ClientValidator : AbstractValidator<Client>
{
    public ClientValidator()
    {
        RuleFor(client => client.Address).NotNull().NotEmpty().MinimumLength(15);
        RuleFor(client => client.CarId).GreaterThan(0);
        RuleFor(client => client.EmployeeId).GreaterThan(0);
        RuleFor(client => client.EmployeeId).GreaterThan(0);
        RuleFor(client => client.Phone).NotNull().Matches(EfCoreSample.Validation.ValidationPatterns.PhonePattern());
    }
}
