using FluentValidation;
using SFSimulator.Core;

namespace SFSimulator.API.Validation.Validators;

public class WeaponValidator : NullableValidator<IWeaponable?>
{
    public WeaponValidator()
    {
        RuleFor(o => o!.MinDmg).GreaterThanOrEqualTo(0).LessThanOrEqualTo(o => o!.MaxDmg);
        RuleFor(o => o!.MaxDmg).GreaterThanOrEqualTo(0);
        RuleFor(o => o!.RuneType).IsInEnum();
        RuleFor(o => o!.RuneValue).InclusiveBetween(0, 60);
    }
}
