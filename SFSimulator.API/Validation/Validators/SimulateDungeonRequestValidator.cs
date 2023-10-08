using FluentValidation;
using SFSimulator.API.Requests;
using SFSimulator.Core;

namespace SFSimulator.API.Validation.Validators;

public class SimulateDungeonRequestValidator : AbstractValidator<SimulateDungeonRequest>
{
    public SimulateDungeonRequestValidator(IDungeonProvider dungeonProvider)
    {
        RuleFor(o => o.Level).InclusiveBetween(1, 800);
        RuleFor(o => o.DungeonPosition).GreaterThan(0).Custom((_, context) => ValidateDungeonExists(dungeonProvider, context));
        RuleFor(o => o.DungeonEnemyPosition).GreaterThan(0);
        RuleFor(o => o.Iterations).InclusiveBetween(1, 10_000_000).GreaterThanOrEqualTo(o => o.WinTreshold);
        RuleFor(o => o.WinTreshold).GreaterThan(0).Custom((winTreshold, context) =>
        {
            if (winTreshold > context.InstanceToValidate.Iterations)
            {
                context.AddFailure(nameof(context.InstanceToValidate.WinTreshold), $"Must be greater than {context.InstanceToValidate.Iterations}");
            }
        });
        RuleFor(o => o.Class).IsInEnum();
        RuleFor(o => o.Strength).GreaterThanOrEqualTo(0);
        RuleFor(o => o.Dexterity).GreaterThanOrEqualTo(0);
        RuleFor(o => o.Intelligence).GreaterThanOrEqualTo(0);
        RuleFor(o => o.Constitution).GreaterThanOrEqualTo(0);
        RuleFor(o => o.Luck).GreaterThanOrEqualTo(0);
        RuleFor(o => o.Armor).GreaterThanOrEqualTo(0);
        RuleFor(o => o.FirstWeapon).SetValidator(new WeaponValidator());
        RuleFor(o => o.SecondWeapon).SetValidator(new WeaponValidator());
        RuleFor(o => o.RuneBonuses).NotNull().SetValidator(new ResistanceRuneBonusesValidator());
        RuleFor(o => o.GladiatorLevel).InclusiveBetween(0, 15);
        RuleFor(o => o.SoloPortal).InclusiveBetween(0, 50);
        RuleFor(o => o.GuildPortal).InclusiveBetween(0, 50);
    }

    private static void ValidateDungeonExists(IDungeonProvider dungeonProvider, ValidationContext<SimulateDungeonRequest> context)
    {
        var instance = context.InstanceToValidate;
        if (!dungeonProvider.IsValidEnemy(instance.DungeonPosition, instance.DungeonEnemyPosition))
        {
            context.AddFailure($"Enemy with provided {nameof(instance.DungeonPosition)} and {nameof(instance.DungeonEnemyPosition)} does not exist");
        }
    }
}
