using FluentValidation;
using SFSimulator.Core;

namespace SFSimulator.API.Validation.Validators;

public class WeaponValidator : NullableValidator<Weapon?>
{
    public WeaponValidator()
    {
        RuleFor(o => o!.MinDmg).GreaterThan(0).LessThanOrEqualTo(o => o!.MaxDmg);
        RuleFor(o => o!.MaxDmg).GreaterThan(0);
        RuleFor(o => o!.DamageRuneType).IsInEnum();
        RuleFor(o => o!.RuneBonus).InclusiveBetween(0, 60);
    }
}
