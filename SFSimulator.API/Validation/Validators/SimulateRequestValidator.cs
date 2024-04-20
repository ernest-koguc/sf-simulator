using FluentValidation;
using SFSimulator.API.Requests;
using SFSimulator.Core;

namespace SFSimulator.API.Validation.Validators;

public class SimulateRequestValidator : AbstractValidator<SimulateRequest>
{
    public SimulateRequestValidator(IGameLogic gameLogic)
    {
        RuleFor(o => o.Level).InclusiveBetween(1, 800);
        RuleFor(o => o.BaseStat).GreaterThan(-1);
        RuleFor(o => o.Type).IsInEnum();
        RuleFor(o => o.CalendarDay).InclusiveBetween(1, 20);
        RuleFor(o => o.Calendar).InclusiveBetween(1, 12);
        RuleFor(o => o.FightsForGold).InclusiveBetween(0, 10000);
        RuleFor(o => o.Experience).GreaterThanOrEqualTo(0).Custom((experience, context) =>
        {
            var level = context.InstanceToValidate.Level;
            if (level <= 0)
            {
                return;
            }
            var maxExperience = gameLogic.GetExperienceForNextLevel(level);
            if (experience > maxExperience - 1)
            {
                context.AddFailure($"Experience for level: {level} can't exceed {maxExperience - 1}");
            }
        });
        RuleFor(o => o.SimulateUntil).Custom((simulateUntil, context) =>
        {
            switch (context.InstanceToValidate.Type)
            {
                case SimulationType.UntilDays:
                    if (simulateUntil is < 1 or > 3000)
                        context.AddFailure($"{nameof(context.InstanceToValidate.SimulateUntil)} must be between inclusive 1 to 3000");
                    break;
                case SimulationType.UntilBaseStats:
                    var baseStats = context.InstanceToValidate.BaseStat;
                    if (simulateUntil <= baseStats)
                        context.AddFailure($"{nameof(context.InstanceToValidate.SimulateUntil)} must be higher than current character's base stats");

                    if (simulateUntil > 1_000_000)
                        context.AddFailure($"{nameof(context.InstanceToValidate.SimulateUntil)} can't exceed 1 000 000");
                    break;
                case SimulationType.UntilLevel:
                    var level = context.InstanceToValidate.Level;
                    if (simulateUntil <= level)
                        context.AddFailure($"{nameof(context.InstanceToValidate.SimulateUntil)} must be higher than current character's level");
                    if (simulateUntil is < 2 or > 800)
                        context.AddFailure($"{nameof(context.InstanceToValidate.SimulateUntil)} must be between 2 to 800");
                    break;
            }
        });
        RuleFor(o => o.ExpeditionOptions).Custom((options, context) =>
        {
            if (options is null && context.InstanceToValidate.ExpeditionsInsteadOfQuests)
            {
                context.AddFailure($"{nameof(context.InstanceToValidate.ExpeditionOptions)} must be provided when {nameof(context.InstanceToValidate.ExpeditionsInsteadOfQuests)} is true");
            }
        });
        RuleFor(o => o.ExpeditionOptionsAfterSwitch).Custom((options, context) =>
        {
            if (options is null && context.InstanceToValidate.ExpeditionsInsteadOfQuests && context.InstanceToValidate.SwitchPriority)
            {
                context.AddFailure($"{nameof(context.InstanceToValidate.ExpeditionOptionsAfterSwitch)} must be provided when {nameof(context.InstanceToValidate.SwitchPriority)} is true");
            }
        });
    }
}
