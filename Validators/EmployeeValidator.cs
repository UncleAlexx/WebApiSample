using EfCoreSample.Entities;
using FluentValidation;

namespace WebApplication3.Validators;
public sealed class EmployeeValidator : AbstractValidator<Employee>
{
    public EmployeeValidator()
    {
        RuleFor(employee => employee.Address).NotNull().NotEmpty().MinimumLength(15);
        RuleFor(employee => employee.FullName).NotNull().NotEmpty().MinimumLength(15);
        RuleFor(employee => employee.Phone).NotNull().Matches(EfCoreSample.Validation.ValidationPatterns.PhonePattern());
        RuleFor(employee => employee.Salary).GreaterThan(0);
        RuleFor(client => client.Post).NotNull().NotEmpty().MinimumLength(10);
    }
}
