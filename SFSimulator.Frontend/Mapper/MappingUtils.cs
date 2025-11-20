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

        simulationContext.Level = maria21DataDto.Level;
        simulationContext.Experience = maria21DataDto.XP;

        simulationContext.GoldPitLevel = maria21DataDto.Underworld.GoldPit;
        simulationContext.HeartOfDarknessLevel = maria21DataDto.Underworld.Heart;
        simulationContext.SoulExtractorLevel = maria21DataDto.Underworld.Extractor;
        simulationContext.UnderworldGateLevel = maria21DataDto.Underworld.Gate;
        simulationContext.TortureChamberLevel = maria21DataDto.Underworld.Torture;
        simulationContext.AdventuromaticLevel = maria21DataDto.Underworld.TimeMachine;
        simulationContext.GladiatorLevel = maria21DataDto.Fortress.Gladiator;
        simulationContext.GoblinPitLevel = maria21DataDto.Underworld.GoblinPit;
        simulationContext.TrollBlockLevel = maria21DataDto.Underworld.TrollBlock;
        simulationContext.KeeperLevel = maria21DataDto.Underworld.Keeper;

        simulationContext.AcademyLevel = maria21DataDto.Fortress.Academy;
        simulationContext.GemMineLevel = maria21DataDto.Fortress.GemMine;
        simulationContext.WorkerLevel = maria21DataDto.Fortress.LaborerQuarters;
        simulationContext.FortressLevel = maria21DataDto.Fortress.Fortress;
        simulationContext.TreasuryLevel = maria21DataDto.Fortress.Treasury;

        simulationContext.Mount = maria21DataDto.Mount;
        simulationContext.Class = maria21DataDto.Class;
        simulationContext.HydraHeads = maria21DataDto.Group.Group?.Hydra ?? 0;

        simulationContext.BaseStrength = maria21DataDto.Strength.Base;
        simulationContext.BaseDexterity = maria21DataDto.Dexterity.Base;
        simulationContext.BaseIntelligence = maria21DataDto.Intelligence.Base;
        simulationContext.BaseConstitution = maria21DataDto.Constitution.Base;
        simulationContext.BaseLuck = maria21DataDto.Luck.Base;

        simulationContext.SoloPortal = maria21DataDto.Dungeons.Player;
        simulationContext.GuildPortal = maria21DataDto.Dungeons.Group;
        simulationContext.Calendar = maria21DataDto.CalendarType;
        simulationContext.CalendarDay = maria21DataDto.CalendarDay == 0 ? 1 : maria21DataDto.CalendarDay;
        simulationContext.Items = MapItems(maria21DataDto.Class, ResolveItemsAsList(maria21DataDto.Items));
        simulationContext.DungeonsData = maria21DataDto.Dungeons;
        simulationContext.Potions = maria21DataDto.Potions;
        simulationContext.Aura = maria21DataDto.Toilet.Aura;
        simulationContext.AuraFillLevel = maria21DataDto.Toilet.Fill;
        simulationContext.RuneQuantity = maria21DataDto.Idle?.Runes ?? 0;
        simulationContext.GuildKnights = maria21DataDto.Group.Group?.TotalKnights ?? 0;
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
                    Items = MapItems(companion.Class, ResolveItemsAsList(companion.Items))
                };
            }).ToArray();
        }

        return simulationContext;
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

    public static List<EquipmentItem> MapItems(ClassType classType, List<SFToolsItem> items)
        => items.Select(item => EquipmentBuilder.FromSFToolsItem(classType, item)).ToList();

    private enum BonusType
    {
        GOLD,
        XP
    }
}