namespace SFSimulator.Core;

public class ExpeditionService(ICurves curves, IItemGenerator itemGenerator) : IExpeditionService
{
    public ExpeditionOptions Options { get; set; } = new ExpeditionOptions(1.5M, 1.28M);
    public decimal GetDailyExpeditionGold(int characterLevel, GoldBonus goldBonus, bool isGoldEvent, MountType mount, int thirst)
    {
        var baseGold = curves.GoldCurve[characterLevel];

        if (isGoldEvent)
        {
            baseGold *= 5;
        }

        baseGold = Math.Min(1E9M, baseGold) / 60.38647M;

        var goldMultiplier = (1 + Math.Min(3, (goldBonus.GuildBonus / 100M) + (goldBonus.Tower / 100M) * 2) + goldBonus.RuneBonus / 100M) * (goldBonus.HasGoldScroll ? 1.1M : 1);
        var goldWithBonuses = Math.Min(40_000_000, baseGold * goldMultiplier) * GetMountBonus(mount);
        goldWithBonuses += Math.Clamp(characterLevel - 557, 0, 75) * (50_000_000 * GetMountBonus(mount) - goldWithBonuses) / 75;
        var goldFromFinalReward = goldWithBonuses;
        var goldFromMidMonster = goldFromFinalReward / 10 * 0.85M;
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
        var xpMultiplier = experienceBonus.CombinedBonus;
        var xp = xpWithStarBonus * xpMultiplier;

        if (isExperienceEvent)
        {
            xp *= 2;
        }

        var totalXp = xp * thirst / 25M;

        return (long)totalXp;
    }

    public List<Item> GetDailyExpeditionItems(int characterLevel, int thirst)
    {
        var amountOfItems = thirst / 25D * 0.5;
        var reminder = amountOfItems % 1;
        amountOfItems = Math.Floor(amountOfItems);
        if (reminder != 0 && Random.Shared.NextDouble() <= reminder)
        {
            amountOfItems++;
        }

        List<Item> items = new();
        for (var i = 0; i < amountOfItems; i++)
        {
            var item = itemGenerator.GenerateItem(characterLevel, ItemSourceType.Expedition);
            items.Add(item);
        }

        return items;
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

public class ExpeditionOptions(decimal AverageAmountOfChests, decimal AverageStarExperienceBonus)
{
    public decimal AverageAmountOfChests { get; set; } = AverageAmountOfChests;
    public decimal AverageStarExperienceBonus { get; set; } = AverageStarExperienceBonus;
}