using FluentValidation;
using SFSimulator.Core;

namespace SFSimulator.Frontend.Validation;


public class SimulationContextValidator : AbstractValidator<SimulationContext>
{
    public SimulationContextValidator()
    {
        RuleFor(o => o.SwitchLevel).InclusiveBetween(2, 1000).Custom((val, context) =>
        {
            if (val <= context.InstanceToValidate.Level)
            {
                context.AddFailure($"{nameof(context.InstanceToValidate.SwitchLevel)} must be greater than {nameof(context.InstanceToValidate.Level)}");
            }
        });
        RuleFor(o => o.GoldBonus.Tower).InclusiveBetween(0, 100);
        RuleFor(o => o.GoldBonus.GuildBonus).InclusiveBetween(0, 200);
        RuleFor(o => o.GoldBonus.RuneBonus).InclusiveBetween(0, 50);
        RuleFor(o => o.ExperienceBonus.ScrapbookFillness).InclusiveBetween(0, 100);
        RuleFor(o => o.ExperienceBonus.GuildBonus).InclusiveBetween(0, 200);
        RuleFor(o => o.ExperienceBonus.RuneBonus).InclusiveBetween(0, 10);
        RuleFor(o => o.DailyThirst).InclusiveBetween(0, 320);
        RuleFor(o => o.SpinAmount).IsInEnum();
        RuleFor(o => o.DailyGuard).InclusiveBetween(0, 24);
        RuleFor(o => o.HydraHeads).InclusiveBetween(0, 20);
        RuleFor(o => o.GoldPitLevel).InclusiveBetween(0, 100);
        RuleFor(o => o.AcademyLevel).InclusiveBetween(0, 20);
        RuleFor(o => o.GemMineLevel).InclusiveBetween(0, 100);
        RuleFor(o => o.TreasuryLevel).InclusiveBetween(0, 45);
        RuleFor(o => o.Mount).IsInEnum();
        RuleFor(o => o.Calendar).InclusiveBetween(0, 20);
        RuleFor(o => o.CalendarDay).InclusiveBetween(0, 20);
        RuleFor(o => o.ExpeditionOptions.AverageAmountOfChests).InclusiveBetween(0, 2);
        RuleFor(o => o.ExpeditionOptions.AverageStarExperienceBonus).InclusiveBetween(1, 1.35M);
        RuleFor(o => o.ExpeditionOptionsAfterSwitch.AverageAmountOfChests).InclusiveBetween(0, 2);
        RuleFor(o => o.ExpeditionOptionsAfterSwitch.AverageStarExperienceBonus).InclusiveBetween(1, 1.35M);
        RuleFor(o => o.ScrollsUnlocked).InclusiveBetween(0, 9);
        RuleFor(o => o.GuildKnights).InclusiveBetween(0, 1000);
        RuleFor(o => o.Level).InclusiveBetween(0, 1000);
        RuleFor(o => o.Class).IsInEnum();
        RuleFor(o => o.Experience).InclusiveBetween(0, 1_500_000_000);
        RuleFor(o => o.BaseStat).InclusiveBetween(0, 10_000_000);
        RuleFor(o => o.Gold).InclusiveBetween(0, 10_000_000_000_000);
        RuleFor(o => o.BaseStrength).InclusiveBetween(0, 10_000_000);
        RuleFor(o => o.BaseDexterity).InclusiveBetween(0, 10_000_000);
        RuleFor(o => o.BaseIntelligence).InclusiveBetween(0, 10_000_000);
        RuleFor(o => o.BaseConstitution).InclusiveBetween(0, 10_000_000);
        RuleFor(o => o.BaseLuck).InclusiveBetween(0, 10_000_000);
        RuleFor(o => o.GladiatorLevel).InclusiveBetween(0, 15);
        RuleFor(o => o.GladiatorLevel).InclusiveBetween(0, 15);
        RuleFor(o => o.GladiatorLevel).InclusiveBetween(0, 15);
        RuleFor(o => o.SoloPortal).InclusiveBetween(0, 50);
        RuleFor(o => o.GuildPortal).InclusiveBetween(0, 50);
        RuleFor(o => o.Aura).InclusiveBetween(0, 66);
        RuleFor(o => o.BlackSmithResources.Splinters).InclusiveBetween(0, 100_000_000);
        RuleFor(o => o.BlackSmithResources.Metal).InclusiveBetween(0, 100_000_000);
        RuleFor(o => o.FinishCondition.FinishWhen).IsInEnum();
        RuleFor(o => o.FinishCondition.Until).Custom((val, context) =>
        {
            switch (context.InstanceToValidate.FinishCondition.FinishWhen)
            {
                case SimulationFinishConditionType.UntilDays:
                    if (val <= 0 || val > 3000)
                    {
                        context.AddFailure($"{nameof(context.InstanceToValidate.FinishCondition.Until)} must be between 1 and 3000");
                    }
                    ;
                    break;
                case SimulationFinishConditionType.UntilLevel:
                    if (val <= context.InstanceToValidate.Level)
                    {
                        context.AddFailure($"{nameof(context.InstanceToValidate.FinishCondition.Until)} must be greater than {nameof(context.InstanceToValidate.Level)}");
                    }
                    ;
                    break;
                case SimulationFinishConditionType.UntilBaseStats:
                    if (val <= context.InstanceToValidate.BaseStat)
                    {
                        context.AddFailure($"{nameof(context.InstanceToValidate.FinishCondition.Until)} must be greater than {nameof(context.InstanceToValidate.BaseStat)}");
                    }
                    ;
                    break;
            }
        });
    }
}
