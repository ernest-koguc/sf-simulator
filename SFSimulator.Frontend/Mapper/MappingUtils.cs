using SFSimulator.Core;

namespace SFSimulator.Frontend;

public static class MappingUtils
{
    public static SimulationContext MapToSimulationContext(SimulationContext simulationContext, Maria21DataDTO maria21DataDto)
    {
        simulationContext.ExperienceBonus.ScrapbookFillness = Math.Round(maria21DataDto.Book / (decimal)CoreShared.SCRAPBOOK_LIMIT * 100, 2);
        simulationContext.ExperienceBonus.GuildBonus = GetGuildBonus(maria21DataDto, BonusType.XP);
        simulationContext.ExperienceBonus.RuneBonus = GetQuestRuneBonus(maria21DataDto, BonusType.XP);
        simulationContext.ExperienceBonus.HasExperienceScroll = maria21DataDto.Items.Head?.Enchantment == WitchScrollType.QuestExperience || maria21DataDto.Inventory.Dummy.Head?.Enchantment == WitchScrollType.QuestExperience;

        simulationContext.GoldBonus.Tower = Math.Max(maria21DataDto.Dungeons.Tower, 0);
        simulationContext.GoldBonus.GuildBonus = GetGuildBonus(maria21DataDto, BonusType.GOLD);
        simulationContext.GoldBonus.RuneBonus = GetQuestRuneBonus(maria21DataDto, BonusType.GOLD);
        simulationContext.GoldBonus.HasGoldScroll = maria21DataDto.Items.Ring?.Enchantment == WitchScrollType.QuestGold || maria21DataDto.Inventory.Dummy.Ring?.Enchantment == WitchScrollType.QuestGold;
        simulationContext.GoldBonus.HasArenaGoldScroll = maria21DataDto.Items.Misc?.Enchantment == WitchScrollType.ArenaGold || maria21DataDto.Inventory.Dummy.Misc?.Enchantment == WitchScrollType.ArenaGold;

        simulationContext.Level = maria21DataDto.Level;
        simulationContext.Experience = maria21DataDto.XP;
        simulationContext.GoldPitLevel = maria21DataDto.Underworld.GoldPit;
        simulationContext.AcademyLevel = maria21DataDto.Fortress.Academy;
        simulationContext.GemMineLevel = maria21DataDto.Fortress.GemMine;
        simulationContext.TreasuryLevel = maria21DataDto.Fortress.Treasury;
        simulationContext.Mount = maria21DataDto.Mount;
        simulationContext.Class = maria21DataDto.Class;
        simulationContext.HydraHeads = maria21DataDto.Group.Group?.Hydra ?? 0;

        simulationContext.BaseStrength = maria21DataDto.Strength.Base;
        simulationContext.BaseDexterity = maria21DataDto.Dexterity.Base;
        simulationContext.BaseIntelligence = maria21DataDto.Intelligence.Base;
        simulationContext.BaseConstitution = maria21DataDto.Constitution.Base;
        simulationContext.BaseLuck = maria21DataDto.Luck.Base;

        simulationContext.GladiatorLevel = maria21DataDto.Fortress.Gladiator;
        simulationContext.SoloPortal = maria21DataDto.Dungeons.Player;
        simulationContext.GuildPortal = maria21DataDto.Dungeons.Group;
        simulationContext.Calendar = maria21DataDto.CalendarType;
        simulationContext.CalendarDay = maria21DataDto.CalendarDay == 0 ? 1 : maria21DataDto.CalendarDay;
        simulationContext.Items = MapItems(maria21DataDto.Class, maria21DataDto.Items);
        simulationContext.DungeonsData = maria21DataDto.Dungeons;
        simulationContext.Potions = maria21DataDto.Potions;
        simulationContext.Aura = maria21DataDto.Toilet.Aura;
        simulationContext.AuraFillLevel = maria21DataDto.Toilet.Fill;
        simulationContext.RuneQuantity = maria21DataDto.Idle?.Runes ?? 0;
        // Is it guild knights or just player knights???
        simulationContext.GuildKnights = maria21DataDto.Fortress.Knights;
        simulationContext.GuildRaids = maria21DataDto.Group.Group?.Raid ?? 0;
        simulationContext.Pets = new PetsState(maria21DataDto.Pets);

        if (maria21DataDto.Companions is null)
        {
            simulationContext.Companions =
            [

                new () { Character = simulationContext, Class = ClassType.Bert, },
                new () { Character = simulationContext, Class = ClassType.Mage, },
                new () { Character = simulationContext, Class = ClassType.Scout, },
            ];
        }
        else
        {
            simulationContext.Companions = new List<SFToolsCompanion> { maria21DataDto.Companions.Bert, maria21DataDto.Companions.Mark, maria21DataDto.Companions.Kunigunde }
            .Select(companion =>
            {
                return new Companion
                {
                    Character = simulationContext,
                    Class = companion.Class == ClassType.Warrior ? ClassType.Bert : companion.Class,
                    Items = MapItems(companion.Class, companion.Items)
                };
            }).ToArray();
        }

        return simulationContext;
    }

    private static Dictionary<(int Week, int Day), ScheduleDay> MapSchedule(Schedule schedule)
    {
        var mappedSchedule = new Dictionary<(int, int), ScheduleDay>();
        var weekId = 1;
        var dayId = 1;
        foreach (var week in schedule.ScheduleWeeks)
        {
            foreach (var day in week.ScheduleDays)
            {
                var actions = MapActions(day.Actions);
                var events = MapEvents(day.Events);
                mappedSchedule[(weekId, dayId)] = new ScheduleDay { Actions = actions, Events = events };
                dayId++;
            }
            weekId++;
            dayId = 1;
        }

        return mappedSchedule;
    }

    private static int CalculateRuneValue(Slots items, RuneType runeType)
    {
        if (runeType is RuneType.None or RuneType.FireDamage or RuneType.ColdDamage or RuneType.LightningDamage)
        {
            return 0;
        }
        var itemsList = ResolveItemsAsList(items);
        int runeValue;
        if (runeType is RuneType.FireResistance or RuneType.ColdResistance or RuneType.LightningResistance)
        {
            runeValue = itemsList.Where(i => i.RuneType == runeType || i.RuneType == RuneType.TotalResistance).Sum(i => i.RuneValue);
        }
        else
        {
            runeValue = itemsList.Where(i => i.RuneType == runeType).Sum(i => i.RuneValue);
        }

        return runeType switch
        {
            RuneType.GoldBonus => Math.Min(50, runeValue),
            RuneType.ExperienceBonus => Math.Min(10, runeValue),
            RuneType.FireResistance => Math.Min(75, runeValue),
            RuneType.ColdResistance => Math.Min(75, runeValue),
            RuneType.LightningResistance => Math.Min(75, runeValue),
            RuneType.EpicChance => Math.Min(50, runeValue),
            RuneType.ItemQuality => Math.Min(5, runeValue),
            RuneType.HealthBonus => Math.Min(15, runeValue),
            _ => 0
        };
    }

    private static List<EventType> MapEvents(List<string> events)
    {
        var list = new List<EventType>();
        foreach (var @event in events)
        {
            var eventEnum = Enum.Parse<EventType>(@event);
            list.Add(eventEnum);
        }

        return list;
    }

    private static List<ActionType> MapActions(List<string> actions)
    {
        var list = new List<ActionType>();
        foreach (var action in actions)
        {
            var actionEnum = Enum.Parse<ActionType>(action);
            list.Add(actionEnum);
        }

        return list;
    }

    private static int GetQuestRuneBonus(Maria21DataDTO dto, BonusType type)
    {
        int runeBonus, runeMax;
        RuneType runeType;
        if (type == BonusType.GOLD)
        {
            runeType = RuneType.GoldBonus;
            runeBonus = dto.Runes.Gold;
            runeMax = 50;
        }
        else
        {
            runeType = RuneType.ExperienceBonus;
            runeBonus = dto.Runes.XP;
            runeMax = 10;
        }

        var dummy = dto.Inventory.Dummy;
        var props = typeof(Slots).GetProperties();

        foreach (var property in props)
        {
            var value = property.GetValue(dummy);
            if (value is SFToolsItem item)
            {
                runeBonus += item.RuneType == runeType ? item.RuneValue : 0;
            }
        }
        runeBonus = Math.Min(runeMax, runeBonus);
        return runeBonus;
    }

    private static int GetGuildBonus(Maria21DataDTO dto, BonusType type)
    {
        if (dto.Group.Group is null)
            return 0;

        var raid = Math.Min(100, dto.Group.Group.Raid * 2);
        var guild = type == BonusType.GOLD ? dto.Group.Group.TotalTreasure : dto.Group.Group.TotalInstructor;
        return Math.Min(200, raid + guild);
    }

    private static int SumBaseStats(Maria21DataDTO dto)
    {
        var con = dto.Constitution.Base;
        return dto.Class switch
        {
            ClassType.Warrior or ClassType.BattleMage or ClassType.Berserker => con + dto.Strength.Base,
            ClassType.Mage or ClassType.Druid or ClassType.Bard or ClassType.Necromancer => con + dto.Intelligence.Base,
            ClassType.Scout or ClassType.Assassin or ClassType.DemonHunter => con + dto.Dexterity.Base,
            _ => con,
        };
    }

    private static List<SFToolsItem> ResolveItemsAsList(Slots items)
    {
        var list = new List<SFToolsItem>
        {
            items.Head,
            items.Body,
            items.Hand,
            items.Feet,
            items.Neck,
            items.Belt,
            items.Ring,
            items.Misc,
            items.Wpn1,
            items.Wpn2
        }.Where(i => i is not null).ToList();

        return list;
    }

    private static List<EquipmentItem> MapItems(ClassType classType, Slots slots)
    {
        var items = ResolveItemsAsList(slots);
        return items.Select(item => EquipmentBuilder.FromSFToolsItem(classType, item)).ToList();
    }

    private enum BonusType
    {
        GOLD,
        XP
    }
}
