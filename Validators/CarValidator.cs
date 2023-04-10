using EfCoreSample.Entities;
using FluentValidation;

namespace WebApplication3.Validators;
public sealed class CarValidator : AbstractValidator<Car>
{
    public CarValidator()
    {
        RuleFor(car => car.Brand).NotNull().NotEmpty().MinimumLength(4);
        RuleFor(car => car.Color).NotNull().NotEmpty().MinimumLength(3);
        RuleFor(car => car.ProviderId).GreaterThan(0);
        RuleFor(car => car.Tires).NotEmpty().NotNull().MinimumLength(4);
        RuleFor(car => car.Type).NotNull().NotEmpty().MinimumLength(5);
        RuleFor(car => car.Weight).GreaterThan(0);
    }
}
