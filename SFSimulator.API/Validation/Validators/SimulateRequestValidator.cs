using FluentValidation;
using SFSimulator.API.Requests;
using SFSimulator.Core;

namespace SFSimulator.API.Validation.Validators;

public class SimulateRequestValidator : AbstractValidator<SimulateRequest>
{
    public SimulateRequestValidator(IGameLogic gameLogic)
    {

        _ = RuleFor(o => o.SimulateUntil).Custom((simulateUntil, context) =>
        {
            switch (context.InstanceToValidate.Type)
            {
                case SimulationFinishCondition.UntilDays:
                    if (simulateUntil is < 1 or > 3000)
                        context.AddFailure($"{nameof(context.InstanceToValidate.SimulateUntil)} must be between inclusive 1 to 3000");
                    break;
                case SimulationFinishCondition.UntilBaseStats:
                    var baseStats = context.InstanceToValidate.BaseStat;
                    if (simulateUntil <= baseStats)
                        context.AddFailure($"{nameof(context.InstanceToValidate.SimulateUntil)} must be higher than current character's base stats");

                    if (simulateUntil > 1_000_000)
                        context.AddFailure($"{nameof(context.InstanceToValidate.SimulateUntil)} can't exceed 1 000 000");
                    break;
                case SimulationFinishCondition.UntilLevel:
                    var level = context.InstanceToValidate.Level;
                    if (simulateUntil <= level)
                        context.AddFailure($"{nameof(context.InstanceToValidate.SimulateUntil)} must be higher than current character's level");
                    if (simulateUntil is < 2 or > 800)
                        context.AddFailure($"{nameof(context.InstanceToValidate.SimulateUntil)} must be between 2 to 800");
                    break;
            }
        });
        _ = RuleFor(o => o.Type).IsInEnum();
        _ = RuleFor(o => o.Level).InclusiveBetween(1, 800);
        _ = RuleFor(o => o.Experience).GreaterThanOrEqualTo(0).Custom((experience, context) =>
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
        _ = RuleFor(o => o.BaseStat).GreaterThanOrEqualTo(0);
        _ = RuleFor(o => o.GoldPitLevel).InclusiveBetween(0, 100);
        _ = RuleFor(o => o.AcademyLevel).InclusiveBetween(0, 20);
        _ = RuleFor(o => o.HydraHeads).InclusiveBetween(0, 20);
        _ = RuleFor(o => o.GemMineLevel).InclusiveBetween(0, 100);
        _ = RuleFor(o => o.TreasuryLevel).InclusiveBetween(0, 45);
        _ = RuleFor(o => o.QuestOptions).Custom((options, context) =>
        {
            if (options is null && !context.InstanceToValidate.ExpeditionsInsteadOfQuests)
            {
                context.AddFailure($"{nameof(context.InstanceToValidate.QuestOptions)} must be provided when {nameof(context.InstanceToValidate.ExpeditionsInsteadOfQuests)} is false");
            }
            if (options is not null && options.Priority == QuestPriorityType.Hybrid && options.HybridRatio is null)
            {
                context.AddFailure($"{nameof(options.HybridRatio)} is required when {nameof(options.Priority)} priority is set to Hybrid");
            }
        });
        _ = RuleFor(o => o.QuestOptionsAfterSwitch).Custom((options, context) =>
        {
            if (options is null && !context.InstanceToValidate.ExpeditionsInsteadOfQuests && context.InstanceToValidate.SwitchPriority)
            {
                context.AddFailure($"{nameof(context.InstanceToValidate.QuestOptions)} must be provided when {nameof(context.InstanceToValidate.SwitchPriority)} is true");
            }
            if (options is not null && options.Priority == QuestPriorityType.Hybrid && options.HybridRatio is null && options is not null)
            {
                context.AddFailure($"{nameof(options.HybridRatio)} is required when {nameof(options.Priority)} priority is set to Hybrid");
            }
        });
        _ = RuleFor(o => o.SwitchLevel).Custom((switchLevel, context) =>
        {
            if (switchLevel is null && context.InstanceToValidate.SwitchPriority)
            {
                context.AddFailure($"{nameof(context.InstanceToValidate.SwitchLevel)} must be provided when {nameof(context.InstanceToValidate.SwitchPriority)} is true");
            }
        });
        _ = RuleFor(o => o.ScrapbookFillness).InclusiveBetween(0, 100);
        _ = RuleFor(o => o.XpGuildBonus).InclusiveBetween(0, 200);
        _ = RuleFor(o => o.XpRuneBonus).InclusiveBetween(0, 10);
        _ = RuleFor(o => o.Tower).InclusiveBetween(0, 100);
        _ = RuleFor(o => o.GoldGuildBonus).InclusiveBetween(0, 200);
        _ = RuleFor(o => o.GoldRuneBonus).InclusiveBetween(0, 50);
        _ = RuleFor(o => o.DailyThirst).InclusiveBetween(0, 320);
        _ = RuleFor(o => o.MountType).IsInEnum();
        _ = RuleFor(o => o.SpinAmount).IsInEnum();
        _ = RuleFor(o => o.DailyGuard).InclusiveBetween(0, 24);
        _ = RuleFor(o => o.CalendarDay).InclusiveBetween(1, 20);
        _ = RuleFor(o => o.Calendar).InclusiveBetween(1, 12);
        _ = RuleFor(o => o.FightsForGold).InclusiveBetween(0, 10000);
        _ = RuleFor(o => o.DrinkExtraWeeklyBeer).Custom((drinkExtraWeeklyBeer, context) =>
        {
            if (drinkExtraWeeklyBeer && !context.InstanceToValidate.DoWeeklyTasks)
            {
                context.AddFailure($"{nameof(context.InstanceToValidate.DrinkExtraWeeklyBeer)} can't be true when {nameof(context.InstanceToValidate.DoWeeklyTasks)} is false");
            }
        });
        _ = RuleFor(o => o.ExpeditionOptions).Custom((options, context) =>
        {
            if (options is null && context.InstanceToValidate.ExpeditionsInsteadOfQuests)
            {
                context.AddFailure($"{nameof(context.InstanceToValidate.ExpeditionOptions)} must be provided when {nameof(context.InstanceToValidate.ExpeditionsInsteadOfQuests)} is true");
            }
        });
        _ = RuleFor(o => o.ExpeditionOptionsAfterSwitch).Custom((options, context) =>
        {
            if (options is null && context.InstanceToValidate.ExpeditionsInsteadOfQuests && context.InstanceToValidate.SwitchPriority)
            {
                context.AddFailure($"{nameof(context.InstanceToValidate.ExpeditionOptionsAfterSwitch)} must be provided when {nameof(context.InstanceToValidate.SwitchPriority)} is true");
            }
        });
        //_ = RuleFor(o => o.DungeonOptions).Custom((options, context) =>
        //{
        //    if (options is null && context.InstanceToValidate.DoDungeons)
        //    {
        //        context.AddFailure($"{nameof(context.InstanceToValidate.DungeonOptions)} must be provided when {nameof(context.InstanceToValidate.DoDungeons)} is true");
        //    }
        //});
        // TODO: validate DungeonOptions subcomponents
    }
}