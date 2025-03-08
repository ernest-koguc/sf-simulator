namespace SFSimulator.Core;

public class ExpeditionService(ICurves curves, IItemGenerator itemGenerator, IGameFormulasService gameFormulasService) : IExpeditionService
{
    public ExpeditionOptions Options { get; set; } = new ExpeditionOptions(1.5M, 1.28M);
    private const decimal MidwayGoldChance = 0.85M;
    private const decimal MidwayFruitBasketChance = 0.3M;
    public double GetDailyExpeditionPetFood(int characterLevel, GoldBonus goldBonus, List<EventType> events, MountType mount, int thirst)
    {
        var midWayGoldReward = gameFormulasService.GetExpeditionMidwayGold(characterLevel, goldBonus, events.Contains(EventType.Gold), mount, thirst);

        // if the gold reward is less than 5 mln than it is better to pick the fruits instead of gold
        var fruitMultiplier = midWayGoldReward < 5_000_000 ? MidwayFruitBasketChance : (1 - MidwayGoldChance) * MidwayFruitBasketChance;
        int expeditions;
        if (thirst > 100)
        {
            expeditions = 4 + (int)Math.Ceiling((thirst - 100) / 20D);
        }
        else
        {
            expeditions = (int)Math.Ceiling(thirst / 25D);
        }

        var midWayBaskets = expeditions * fruitMultiplier;
        var fruits = midWayBaskets * 5;

        if (events.Contains(EventType.Pets))
        {
            var finalMidWayBaskets = expeditions / 50;
            if (mount == MountType.Griffin)
            {
                finalMidWayBaskets *= 2;
            }

            fruits += finalMidWayBaskets * 5;
        }

        return (double)fruits;
    }

    public decimal GetDailyExpeditionGold(int characterLevel, GoldBonus goldBonus, bool isGoldEvent, MountType mount, int thirst)
    {
        var goldFromFinalReward = gameFormulasService.GetExpeditionFinalGold(characterLevel, goldBonus, isGoldEvent, mount, thirst);

        var goldFromMidMonster = gameFormulasService.GetExpeditionMidwayGold(characterLevel, goldBonus, isGoldEvent, mount, thirst);
        if (goldFromMidMonster < 5_000_000)
        {
            // If the gold is less than 5 mln than it is better to pick pet fruits so we reduce the gold by the chance of getting the fruit basket
            goldFromMidMonster *= (1 - MidwayFruitBasketChance) * MidwayGoldChance;
        }
        else
        {
            goldFromMidMonster *= MidwayGoldChance;
        }

        var goldPerChest = gameFormulasService.GetExpeditionChestGold(characterLevel, goldBonus, isGoldEvent, mount, thirst);

        var totalGold = (goldFromFinalReward + goldFromMidMonster + goldPerChest * Options.AverageAmountOfChests);

        return totalGold;
    }

    public long GetDailyExpeditionExperience(int characterLevel, ExperienceBonus experienceBonus, bool isExperienceEvent, MountType mount, int thirst)
    {
        var baseExperience = curves.ExperienceCurve[characterLevel] / (0.75M * (characterLevel + 1)) / 11 / (decimal)Math.Max(1, Math.Exp(30090.33D / 5000000D * (characterLevel - 99)));
        baseExperience *= 15.18M;
        var xpWithMount = baseExperience * GetMountBonus(mount);
        var xpWithStarBonus = xpWithMount * Options.AverageStarExperienceBonus;
        var xpMultiplier = (1 + (experienceBonus.GuildBonus + experienceBonus.ScrapbookFillness + experienceBonus.RuneBonus) / 100M);
        if (experienceBonus.HasExperienceScroll)
        {
            xpMultiplier *= 1.1M;
        }
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
            var item = itemGenerator.GenerateItem(characterLevel);
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
    public ExpeditionOptions() : this(1.28M, 1.2M) { }
    public decimal AverageAmountOfChests { get; set; } = AverageAmountOfChests;
    public decimal AverageStarExperienceBonus { get; set; } = AverageStarExperienceBonus;
}