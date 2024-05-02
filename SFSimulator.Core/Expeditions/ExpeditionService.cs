namespace SFSimulator.Core;

public class ExpeditionService(ICurves curves) : IExpeditionService
{
    public ExpeditionOptions Options { get; set; } = new ExpeditionOptions(1.5M, 1.28M);
    public decimal GetDailyExpeditionGold(int characterLevel, GoldBonus goldBonus, bool isGoldEvent, MountType mount, int thirst)
    {
        var baseGold = curves.GoldCurve[characterLevel];

        if (isGoldEvent)
        {
            baseGold *= 4;
        }

        baseGold = Math.Min(1E9M, baseGold) / 60.38647M;

        var goldMultiplier = (1 + Math.Min(3, goldBonus.GuildBonus + goldBonus.Tower * 2) + goldBonus.RuneBonus) * (1 + (goldBonus.HasGoldScroll ? 0.1M : 0));
        var goldWithBonuses = Math.Min(50_000_000, baseGold * goldMultiplier);
        var goldFromFinalReward = goldWithBonuses * GetMountBonus(mount);
        var goldFromMidMonster = goldFromFinalReward / 10;
        var goldPerChest = goldFromFinalReward / 5;

        var totalGold = (goldFromFinalReward + goldFromMidMonster + goldPerChest * Options.AverageAmountOfChests) * thirst / 25M;

        return totalGold;
    }

    public long GetDailyExpeditionExperience(int characterLevel, ExperienceBonus experienceBonus, bool isExperienceEvent, MountType mount, int thirst)
    {
        var baseExperience = curves.ExperienceCurve[characterLevel] / (0.75M * (characterLevel + 1)) / 11 / (decimal)Math.Max(1, Math.Exp(30090.33D / 5000000D * (characterLevel - 99)));
        baseExperience *= 15.18M;
        var xpWithMount = baseExperience * GetMountBonus(mount);
        var xpWithStarBonus = xpWithMount * Options.AverageStarExperienceBonus;
        var xpMultiplier = (1 + experienceBonus.GuildBonus + experienceBonus.ScrapbookFillness + experienceBonus.RuneBonus) * (1 + (experienceBonus.HasExperienceScroll ? 0.1 : 0));
        var xp = xpWithStarBonus * (decimal)xpMultiplier;

        if (isExperienceEvent)
        {
            xp *= 2;
        }

        var totalXp = xp * thirst / 25M;

        return (long)totalXp;
    }

    private static decimal GetMountBonus(MountType mount)
        => mount switch
        {
            MountType.None => 1,
            MountType.Pig => 1.11M,
            MountType.Horse => 1.25M,
            MountType.Tiger => 1.42M,
            MountType.Griffin => 2,
            _ => throw new ArgumentOutOfRangeException(nameof(mount))
        };
}

public readonly record struct ExpeditionOptions(decimal AverageAmountOfChests, decimal AverageStarExperienceBonus);


