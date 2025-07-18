﻿namespace SFSimulator.Core;

public class GameFormulasService : IGameFormulasService
{
    private ICurves Curves { get; init; }

    public GameFormulasService(ICurves curves)
    {
        Curves = curves ?? throw new ArgumentNullException(nameof(curves));
    }
    public long GetExperienceForNextLevel(int characterLevel)
    {
        if (characterLevel > 393) return 1500000000;
        return Curves.ExperienceCurve[characterLevel];
    }
    public long GetDailyMissionExperience(int characterLevel, bool xpEvent, int hydraHeads = 0)
    {
        var xp = GetExperienceForNextLevel(characterLevel);
        var multiplier = 1.5 + 0.75 * (characterLevel - 1);
        var basic = xp / multiplier;
        var daily = Convert.ToInt32(Math.Floor((1 + 0.25 * hydraHeads) * basic));
        if (xpEvent)
            daily *= 2;

        return daily;
    }
    public decimal GetDailyMissionGold(int characterLevel)
    {
        var baseGold = GetGoblinTasksBaseGold(characterLevel);
        return Math.Floor(baseGold * 16);
    }
    public decimal GetGoblinTasksBaseGold(int characterLevel)
    {
        var gold = Curves.GoldCurve[characterLevel] / 100;
        return gold;
    }
    public long GetWeeklyTasksExperience(int characterLevel, ExperienceBonus experienceBonus)
    {
        var baseXp = GetExperienceForNextLevel(characterLevel);
        var multiplier = 1.5 + 0.75 * (characterLevel - 1);
        var xp = baseXp / multiplier;

        var xpReward = Math.Truncate(xp / Math.Max(1, Math.Exp(30090.33 / 5000000 * (characterLevel - 99))));

        xpReward *= 2.456;
        var xpBonus = experienceBonus.ScrapbookPlusGuildPlusRuneBonus;

        return (long)((decimal)xpReward * xpBonus);
    }

    public QuestValue GetMinimumQuestValue(int characterLevel, ExperienceBonus experienceBonus, GoldBonus goldBonus)
    {
        var xp = GetExperienceForNextLevel(characterLevel) / (1.5 + 0.75 * (characterLevel - 1));
        var xpMin = (int)((double)experienceBonus.CombinedBonus * xp / 11 / Math.Max(1, Math.Exp(30090.33 / 5000000 * (characterLevel - 99))));
        var gold = Curves.GoldCurve[characterLevel] * 12 / 1000;
        var goldMin = goldBonus.TowerPlusGuildBonus * (gold / 11);
        return new QuestValue(goldMin, xpMin);
    }

    public decimal GetGoldFromGuardDuty(int characterLevel, GoldBonus? goldBonus, bool goldEvent)
    {
        var hourlyGuard = Curves.GoldCurve[characterLevel] * 12 / 1000 / 3;

        if (goldBonus != null)
            hourlyGuard *= goldBonus.TowerPlusGuildBonus;

        if (goldEvent)
            hourlyGuard *= 5;

        if (hourlyGuard > 10000000)
            return 10000000;

        return hourlyGuard;
    }

    public decimal GetHourlyGoldPitProduction(int characterLevel, int goldPitLevel, bool goldEvent)
    {
        var goldCurveValue = Curves.GoldCurve[characterLevel] * 12 / 1000;
        var goldPitProduction = goldCurveValue * goldPitLevel / 75M * (1 + Math.Max(0, goldPitLevel - 15) / 100M);

        if (goldEvent)
            goldPitProduction *= 1.5M;

        return goldPitProduction;
    }

    public long GetAcademyHourlyProduction(int characterLevel, int academyLevel, bool xpEvent)
    {
        var xp = GetExperienceForNextLevel(characterLevel);
        var multiplier = 1.5 + 0.75 * (characterLevel - 1);
        var basic = xp / multiplier;
        var hbase = basic / 30 / Math.Max(1, Math.Exp(30090.33 / 5000000 * (characterLevel - 99)));
        var hourly = (int)(academyLevel * hbase);

        if (xpEvent)
            hourly *= 2;

        return hourly;
    }

    public long GetExperienceRewardFromArena(int characterLevel, bool xpEvent)
    {
        var xp = GetExperienceForNextLevel(characterLevel);
        var multiplier = 1.5 + 0.75 * (characterLevel - 1);
        var basic = xp / multiplier;
        var arena = (int)basic / 10;

        if (xpEvent)
            arena *= 2;

        return arena;
    }

    public decimal GetGoldRewardFromArena(int characterLevel, int count, bool arenaScroll)
    {
        decimal scrollMultiplier = arenaScroll ? 1.2M : 1;
        if (characterLevel <= 95)
            return Math.Truncate(scrollMultiplier * characterLevel) * count;

        var baseValue = Curves.GoldCurve[characterLevel] / 50;

        var levelMultiplier = characterLevel switch
        {
            >= 300 => 10,
            >= 250 => 15,
            >= 200 => 20,
            >= 150 => 24,
            _ => 28
        };

        var gold = Math.Truncate(baseValue / levelMultiplier * scrollMultiplier) * count;
        return Math.Min(gold, 1E8M);
    }

    public long GetExperienceRewardFromCalendar(int characterLevel, int rewardSize)
    {
        if (rewardSize < 1 || rewardSize > 3)
            throw new ArgumentOutOfRangeException(nameof(rewardSize));

        var xp = GetExperienceForNextLevel(characterLevel);
        var multiplier = 5d * (4 - rewardSize);
        var calendar = Math.Ceiling(xp / multiplier);

        return (int)calendar;
    }

    public decimal GetGoldRewardFromCalendar(int characterLevel, int rewardSize)
    {
        if (rewardSize < 1 || rewardSize > 2)
            throw new ArgumentOutOfRangeException(nameof(rewardSize));

        var goldCurveMultiplier = Curves.GoldCurve[characterLevel] * 12 / 1000 / 3;
        var reward = goldCurveMultiplier * 25;

        if (rewardSize == 1)
            reward = 3 * reward / 10;

        return reward;
    }

    public decimal GetDailyGoldFromDiceGame(int characterLevel, IEnumerable<EventType> events)
    {
        var averageGold = 0.3555M;

        var dailySpins = 10;

        if (events.Contains(EventType.LuckyDay))
            dailySpins = 20;


        var goldFromGuard = GetGoldFromGuardDuty(characterLevel, null, false);
        var goldReward = goldFromGuard * dailySpins * averageGold;

        if (events.Contains(EventType.Gold))
            goldReward *= 5;

        return goldReward;
    }

    public decimal GetDailyGoldFromWheel(int characterLevel, IEnumerable<EventType> events, SpinAmountType spinAmount)
    {
        var isLuckyDay = events.Contains(EventType.LuckyDay);

        var spins = 1;

        if (isLuckyDay && spinAmount == SpinAmountType.Max)
            spins = 40;
        else if (spinAmount == SpinAmountType.Max)
            spins = 20;

        var goldRewardChance = 0.1M;

        var goldFromGuard = GetGoldFromGuardDuty(characterLevel, null, false);
        var goldReward = goldFromGuard * 20;

        if (goldReward > 15000000)
            goldReward = 15000000;
        if (events.Contains(EventType.Gold))
            goldReward *= 5;

        var gold = goldReward * spins * goldRewardChance;

        return gold;
    }

    public long GetDailyExperienceFromWheel(int characterLevel, IEnumerable<EventType> events, SpinAmountType spinAmount)
    {
        var isLuckyDay = events.Contains(EventType.LuckyDay);

        var spins = 1;

        if (isLuckyDay && spinAmount == SpinAmountType.Max)
            spins = 40;
        else if (spinAmount == SpinAmountType.Max)
            spins = 20;

        var xp = GetExperienceForNextLevel(characterLevel);
        var multiplier = 1.5 + 0.75 * (characterLevel - 1);
        var basic = xp / multiplier;

        var xpReward = Math.Truncate(basic / Math.Max(1, Math.Exp(30090.33 / 5000000 * (characterLevel - 99))));

        var smallXpRewardChance = 0.1f;
        var bigXpRewardChance = 0.1f;

        var totalXp = spins * smallXpRewardChance * xpReward / 2 + spins * bigXpRewardChance * xpReward;

        if (events.Contains(EventType.Experience))
            totalXp *= 2;

        return (int)totalXp;
    }
    public long GetXPFromGuildFight(int characterLevel, IEnumerable<EventType> events)
    {
        var xpBase = GetExperienceForNextLevel(characterLevel);
        var xp = Math.Floor(15 / 16d * xpBase / (characterLevel + 5d));

        if (events.Contains(EventType.Experience))
            xp *= 2;

        return (int)xp;
    }

    public decimal GetDailyGoldFromGemMine(int characterLevel, int gemMineLevel, int workers = 15)
    {
        if (gemMineLevel == 0)
            return 0;

        var time = (1 - workers * 0.05M) * GetMiningDuration(gemMineLevel) * 60;
        var gold = 24 * 60 / time * GetAverageGemGold(characterLevel, gemMineLevel);

        return gold;
    }
    private decimal GetAverageGemGold(int characterLevel, int gemMineLevel)
    {
        var goldCurve = Curves.GoldCurve[characterLevel] * 12 / 1000;

        var gem = goldCurve / 6;
        gem = Math.Min(10000000, gem * GetGemMineMultiplier(gemMineLevel));

        var gem2 = Curves.GoldCurve[characterLevel + 5] / 10;
        gem2 = Math.Min(10E6M, gem2 * 2 * GetGemMineMultiplier(gemMineLevel) / 100);

        var gem3 = Curves.GoldCurve[characterLevel + 10] / 10;
        gem3 = Math.Min(10E6M, gem3 * 2 * GetGemMineMultiplier(gemMineLevel) / 100);

        var avgValue = (gem + gem2 + gem3) / 3;

        return avgValue;
    }
    private static int GetGemMineMultiplier(int gemMineLevel)
    {
        if (gemMineLevel <= 1)
        {
            return 1;
        }
        else if (gemMineLevel >= 14)
        {
            return 24;
        }
        else
        {
            switch (gemMineLevel)
            {
                case 2: return 2;
                case 3: return 3;
                case 4: return 4;
                case 5: return 6;
                case 6: return 8;
                case 7: return 10;
                case 8: return 12;
                case 9: return 14;
                case 10: return 16;
                case 11: return 18;
                case 12: return 20;
                case 13: return 22;
            }
        }
        return 11;
    }
    private static int GetMiningDuration(int gemMineLevel)
    {
        return gemMineLevel switch
        {
            1 => 1,
            2 => 2,
            3 => 3,
            4 => 4,
            5 => 6,
            6 => 8,
            7 => 10,
            8 => 12,
            9 => 14,
            10 => 16,
            11 => 18,
            12 => 20,
            13 => 24,
            14 => 28,
            15 => 32,
            16 => 30,
            17 => 28,
            18 => 26,
            19 => 25,
            _ => 25,
        };
    }

    public long GetExperienceForDungeonEnemy(DungeonEnemy dungeonEnemy)
    {
        if (dungeonEnemy.Dungeon.Type == DungeonTypeEnum.Tower)
            return 0;

        long xp;
        if (dungeonEnemy.Level >= 393)
        {
            xp = 300_000_000;
        }
        else
        {
            xp = Curves.ExperienceCurve[dungeonEnemy.Level] / 5;
        }

        if (dungeonEnemy.Dungeon.Type == DungeonTypeEnum.Twister)
            xp /= 10;

        return xp;
    }

    public long GetExperienceForPetDungeonEnemy(int characterLevel)
    {
        var xpBase = characterLevel >= 393 ? 1_500_000_000D : (double)Curves.ExperienceCurve[characterLevel];
        var xpReducedBase = Math.Max(1, Math.Exp(30090.33 / 5000000 * (characterLevel - 99)));

        var xp = 6 * xpBase / xpReducedBase / 30;

        return (long)xp;
    }

    public double GetDailyPetFoodFromWheel(int characterLevel, List<EventType> events, SpinAmountType spinAmount)
    {
        if (characterLevel < 125)
        {
            return 0;
        }

        var isLuckyDay = events.Contains(EventType.LuckyDay);

        var spins = 1;

        if (isLuckyDay && spinAmount == SpinAmountType.Max)
            spins = 40;
        else if (spinAmount == SpinAmountType.Max)
            spins = 20;

        var foodChance = 0.1;

        var food = spins * foodChance;
        if (events.Contains(EventType.Pets))
            food *= 3;

        return food;
    }

    public decimal GetExpeditionChestGold(int characterLevel, GoldBonus goldBonus, bool isGoldEvent, MountType mount, decimal thirst)
    {
        var baseGold = Curves.GoldCurve[characterLevel];

        if (isGoldEvent)
        {
            baseGold *= 5;
        }

        baseGold = Math.Min(1E9M, baseGold) / 60.38647M;

        var goldMultiplier = (1 + Math.Min(3, (goldBonus.GuildBonus / 100M) + (goldBonus.Tower / 100M) * 2) + goldBonus.RuneBonus / 100M) * (goldBonus.HasGoldScroll ? 1.1M : 1);
        var goldWithBonuses = Math.Min(40_000_000, baseGold * goldMultiplier) * GetExpeditionMountBonus(mount);
        goldWithBonuses += Math.Clamp(characterLevel - 557, 0, 75) * (50_000_000 * GetExpeditionMountBonus(mount) - goldWithBonuses) / 75;
        var goldPerChest = goldWithBonuses / 5 * thirst / 25M;

        return goldPerChest;
    }

    public decimal GetExpeditionMidwayGold(int characterLevel, GoldBonus goldBonus, bool isGoldEvent, MountType mount, decimal thirst)
    {
        var baseGold = Curves.GoldCurve[characterLevel];

        if (isGoldEvent)
        {
            baseGold *= 5;
        }

        baseGold = Math.Min(1E9M, baseGold) / 60.38647M;

        var goldMultiplier = (1 + Math.Min(3, (goldBonus.GuildBonus / 100M) + (goldBonus.Tower / 100M) * 2) + goldBonus.RuneBonus / 100M) * (goldBonus.HasGoldScroll ? 1.1M : 1);
        var goldWithBonuses = Math.Min(40_000_000, baseGold * goldMultiplier) * GetExpeditionMountBonus(mount);
        goldWithBonuses += Math.Clamp(characterLevel - 557, 0, 75) * (50_000_000 * GetExpeditionMountBonus(mount) - goldWithBonuses) / 75;
        var goldFromMidMonster = goldWithBonuses / 10 * thirst / 25M;

        return goldFromMidMonster;
    }

    public decimal GetExpeditionFinalGold(int characterLevel, GoldBonus goldBonus, bool isGoldEvent, MountType mount, decimal thirst)
    {
        var baseGold = Curves.GoldCurve[characterLevel];

        if (isGoldEvent)
        {
            baseGold *= 5;
        }

        baseGold = Math.Min(1E9M, baseGold) / 60.38647M;

        var goldMultiplier = (1 + Math.Min(3, (goldBonus.GuildBonus / 100M) + (goldBonus.Tower / 100M) * 2) + goldBonus.RuneBonus / 100M) * (goldBonus.HasGoldScroll ? 1.1M : 1);
        var goldWithBonuses = Math.Min(40_000_000, baseGold * goldMultiplier) * GetExpeditionMountBonus(mount);
        goldWithBonuses += Math.Clamp(characterLevel - 557, 0, 75) * (50_000_000 * GetExpeditionMountBonus(mount) - goldWithBonuses) / 75;
        var goldFromFinalReward = goldWithBonuses * thirst / 25M;

        return goldFromFinalReward;
    }


    private static decimal GetExpeditionMountBonus(MountType mount)
    => mount switch
    {
        MountType.None => 1,
        MountType.Pig => 1.11M,
        MountType.Horse => 1.25M,
        MountType.Tiger => 1.42M,
        MountType.Griffin => 2,
        _ => throw new ArgumentOutOfRangeException(nameof(mount))
    };

    public decimal GetGoldForDungeonEnemy(DungeonEnemy dungeonEnemy)
    {
        if (DoesDungeonEnemyDropItem(dungeonEnemy))
        {
            return 0;
        }

        if (dungeonEnemy.Dungeon.Type == DungeonTypeEnum.Tower)
        {
            return 9 * Curves.GoldCurve[dungeonEnemy.Level + 2] / 100;
        }

        if (dungeonEnemy.Dungeon.Type is DungeonTypeEnum.Twister or DungeonTypeEnum.Sandstorm or DungeonTypeEnum.Default)
        {
            return 2 * Curves.GoldCurve[dungeonEnemy.Level] / 100;
        }

        return 0;
    }

    public bool DoesDungeonEnemyDropItem(DungeonEnemy dungeonEnemy)
        => (dungeonEnemy.Dungeon.Type == DungeonTypeEnum.Default && dungeonEnemy.Position is 3 or 5 or 7 or 10)
        || (dungeonEnemy.Dungeon.Type is DungeonTypeEnum.Twister or DungeonTypeEnum.Sandstorm && dungeonEnemy.Position % 2 == 0)
        || (dungeonEnemy.Dungeon.Type is DungeonTypeEnum.Default && dungeonEnemy.Dungeon.Position is 15 or 19 or 22 or 26);

    public decimal GetMinimumExpeditionLength(int characterLevel)
    {
        return characterLevel switch
        {
            >= 101 => 25,
            >= 13 => 12.5M,
            12 => 11.25M,
            11 => 10,
            10 => 8.75M,
            9 => 8.33M,
            8 => 5,
            7 => 3.75M,
            6 => 2.5M,
            5 => 2.25M,
            4 => 2,
            3 => 1.75M,
            2 => 1.5M,
            1 => 1.25M,
            _ => throw new ArgumentOutOfRangeException(nameof(characterLevel), $"{nameof(characterLevel)} greater or equal to 1")
        };
    }

    public decimal GetGoldPitCapacity(int characterLevel, int goldPitLevel)
    {
        if (goldPitLevel == 0)
        {
            return 0;
        }
        ArgumentOutOfRangeException.ThrowIfGreaterThan(goldPitLevel, 100);
        ArgumentOutOfRangeException.ThrowIfLessThan(goldPitLevel, 1);

        var goldPitHourly = GetHourlyGoldPitProduction(characterLevel, goldPitLevel, false);
        return Math.Floor(Math.Min(300_000_000, goldPitHourly * goldPitLevel * Math.Max(2, 12M / goldPitLevel)));
    }

    public long GetAcademyCapacity(int characterLevel, int academyLevel)
    {
        if (academyLevel == 0)
        {
            return 0;
        }

        var hourlyAcademy = GetAcademyHourlyProduction(characterLevel, academyLevel, false);
        var academyMultiplier = academyLevel switch
        {
            1 => 1,
            2 => 1.1,
            3 => 1.2,
            4 => 1.3,
            5 => 1.4,
            6 => 1.5,
            7 => 1.6,
            8 => 2.0,
            9 => 2.4,
            10 => 3.2,
            11 => 4.0,
            12 => 4.8,
            13 => 6.4,
            14 => 8.0,
            15 => 9.6,
            16 => 10.0,
            17 => 10.4,
            18 => 10.8,
            19 => 11.2,
            20 => 12,
            _ => throw new ArgumentOutOfRangeException(nameof(academyLevel), $"{nameof(academyLevel)} must be between 1 and 20")
        };

        return (long)(hourlyAcademy * academyMultiplier);
    }
}
