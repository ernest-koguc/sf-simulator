using FluentValidation;
using SFSimulator.Core;

namespace SFSimulator.API.Validation.Validators;

public class ResistanceRuneBonusesValidator : AbstractValidator<ResistanceRuneBonuses>
{
    public ResistanceRuneBonusesValidator()
    {
        RuleFor(o => o.FireResistance).InclusiveBetween(0, 75);
        RuleFor(o => o.LightningResistance).InclusiveBetween(0, 75);
        RuleFor(o => o.ColdResistance).InclusiveBetween(0, 75);
        RuleFor(o => o.HealthRune).InclusiveBetween(0, 15);
    }
}
