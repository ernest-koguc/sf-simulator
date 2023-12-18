using FluentValidation;
using SFSimulator.Core;

namespace SFSimulator.API.Validation.Validators;

public class RawCompanionValidator: AbstractValidator<RawCompanion>
{
    public RawCompanionValidator() 
    {
        RuleFor(o => o.Class).IsInEnum();
        RuleFor(o => o.Strength).GreaterThanOrEqualTo(0);
        RuleFor(o => o.Dexterity).GreaterThanOrEqualTo(0);
        RuleFor(o => o.Intelligence).GreaterThanOrEqualTo(0);
        RuleFor(o => o.Constitution).GreaterThanOrEqualTo(0);
        RuleFor(o => o.Luck).GreaterThanOrEqualTo(0);
        RuleFor(o => o.Armor).GreaterThanOrEqualTo(0);
        RuleFor(o => o.SoloPortal).InclusiveBetween(0, 50);
        RuleFor(o => o.GuildPortal).InclusiveBetween(0, 50);
        RuleFor(o => o.Reaction).InclusiveBetween(0, 1);
        RuleFor(o => o.FirstWeapon).SetValidator(new WeaponValidator());
        RuleFor(o => o.SecondWeapon).SetValidator(new WeaponValidator());
        RuleFor(o => o.LightningResistance).InclusiveBetween(0, 75);
        RuleFor(o => o.FireResistance).InclusiveBetween(0, 75);
        RuleFor(o => o.ColdResistance).InclusiveBetween(0, 75);
        RuleFor(o => o.HealthRune).InclusiveBetween(0, 15);
    }
}

