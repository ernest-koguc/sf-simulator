using AutoMapper;
using SFSimulator.Core;
using static SFSimulator.API.Mappings.StaticMappingFunctions;

namespace SFSimulator.API.Mappings;

public class SimulatorMappingProfiler : Profile
{
    public SimulatorMappingProfiler()
    {
        CreateMap<Maria21DataDTO, EndpointDataDTO>()
            .ForMember(d => d.CharacterName, m => m.MapFrom(s => s.Name + "@" + s.Prefix.Replace(" ", string.Empty)))
            .ForMember(d => d.ScrapbookFillness, m => m.MapFrom(s => Math.Round(s.Book / 2305f * 100, 3)))
            .ForMember(d => d.Level, m => m.MapFrom(s => s.Level))
            .ForMember(d => d.Experience, m => m.MapFrom(s => s.XP))
            .ForMember(d => d.GoldPitLevel, m => m.MapFrom(s => s.Underworld.GoldPit))
            .ForMember(d => d.AcademyLevel, m => m.MapFrom(s => s.Fortress.Academy))
            .ForMember(d => d.GemMineLevel, m => m.MapFrom(s => s.Fortress.GemMine))
            .ForMember(d => d.TreasuryLevel, m => m.MapFrom(s => s.Fortress.Treasury))
            .ForMember(d => d.Tower, m => m.MapFrom(s => Math.Max(s.Dungeons.Tower, 0)))
            .ForMember(d => d.XpRuneBonus, m => m.MapFrom(s => GetQuestRuneBonus(s, BonusType.XP)))
            .ForMember(d => d.GoldRuneBonus, m => m.MapFrom(s => GetQuestRuneBonus(s, BonusType.GOLD)))
            .ForMember(d => d.MountType, m => m.MapFrom(s => s.Mount))
            .ForMember(d => d.HasGoldScroll, m => m.MapFrom(s => s.Items.Ring.Enchantment == WitchScrollType.QuestGold || s.Inventory.Dummy.Ring.Enchantment == WitchScrollType.QuestGold))
            .ForMember(d => d.HasArenaGoldScroll, m => m.MapFrom(s => s.Items.Misc.Enchantment == WitchScrollType.ArenaGold || s.Inventory.Dummy.Misc.Enchantment == WitchScrollType.ArenaGold))
            .ForMember(d => d.HasExperienceScroll, m => m.MapFrom(s => s.Items.Head.Enchantment == WitchScrollType.QuestExperience || s.Inventory.Dummy.Head.Enchantment == WitchScrollType.QuestExperience))
            .ForMember(d => d.BaseStat, m => m.MapFrom(s => SumBaseStats(s)))
            .ForMember(d => d.HydraHeads, m => m.MapFrom(s => s.Group.Group!.Hydra))
            .ForMember(d => d.GoldGuildBonus, m => m.MapFrom(s => GetGuildBonus(s, BonusType.GOLD)))
            .ForMember(d => d.XpGuildBonus, m => m.MapFrom(s => GetGuildBonus(s, BonusType.XP)))
            .ForMember(d => d.Class, m => m.MapFrom(s => s.Class))
            .ForMember(d => d.Strength, m => m.MapFrom(s => s.Strength.Base))
            .ForMember(d => d.Dexterity, m => m.MapFrom(s => s.Dexterity.Base))
            .ForMember(d => d.Intelligence, m => m.MapFrom(s => s.Intelligence.Base))
            .ForMember(d => d.Constitution, m => m.MapFrom(s => s.Constitution.Base))
            .ForMember(d => d.Luck, m => m.MapFrom(s => s.Luck.Base))
            .ForMember(d => d.FirstWeapon, m => m.MapFrom(s => s.Items.Wpn1))
            .ForMember(d => d.SecondWeapon, m => m.MapFrom(s => s.Items.Wpn2))
            .ForMember(d => d.GladiatorLevel, m => m.MapFrom(s => s.Fortress.Gladiator))
            .ForMember(d => d.SoloPortal, m => m.MapFrom(s => s.Dungeons.Player))
            .ForMember(d => d.GuildPortal, m => m.MapFrom(s => s.Dungeons.Group))
            .ForMember(d => d.Calendar, m => m.MapFrom(s => s.CalendarType))
            .ForMember(d => d.CalendarDay, m => m.MapFrom(s => s.CalendarDay == 0 ? 1 : s.CalendarDay))
            .ForMember(d => d.Items, m => m.MapFrom(s => ResolveItemsAsList(s.Items)))
            ;

        CreateMap<Maria21DataDTO, RawFightable>()
            .ForMember(d => d.Level, m => m.MapFrom(s => s.Level))
            .ForMember(d => d.Class, m => m.MapFrom(s => s.Class))
            .ForMember(d => d.FirstWeapon, m => m.MapFrom(s => s.Items.Wpn1))
            .ForMember(d => d.SecondWeapon, m => m.MapFrom(s => s.Items.Wpn2))
            .ForMember(d => d.GladiatorLevel, m => m.MapFrom(s => s.Fortress.Gladiator))
            .ForMember(d => d.SoloPortal, m => m.MapFrom(s => s.Dungeons.Player))
            .ForMember(d => d.GuildPortal, m => m.MapFrom(s => s.Dungeons.Group))
            .ForMember(d => d.Strength, m => m.MapFrom(s => s.Strength.Total))
            .ForMember(d => d.Dexterity, m => m.MapFrom(s => s.Dexterity.Total))
            .ForMember(d => d.Intelligence, m => m.MapFrom(s => s.Intelligence.Total))
            .ForMember(d => d.Constitution, m => m.MapFrom(s => s.Constitution.Total))
            .ForMember(d => d.Luck, m => m.MapFrom(s => s.Luck.Total))
            .ForMember(d => d.HasEternityPotion, m => m.MapFrom(s => s.Potions.Any(p => p.Type == PotionType.Eternity)))
            .ForMember(d => d.HasWeaponScroll, m => m.MapFrom(s => s.Items.Wpn1.Enchantment == WitchScrollType.Crit || s.Items.Wpn2.Enchantment == WitchScrollType.Crit))
            .ForMember(d => d.Reaction, m => m.MapFrom(s => s.Items.Hand.Enchantment == WitchScrollType.Reaction ? 1 : 0))
            .ForMember(d => d.FireResistance, m => m.MapFrom(s => CalculateRuneValue(s.Items, RuneType.FireResistance)))
            .ForMember(d => d.ColdResistance, m => m.MapFrom(s => CalculateRuneValue(s.Items, RuneType.ColdResistance)))
            .ForMember(d => d.LightningResistance, m => m.MapFrom(s => CalculateRuneValue(s.Items, RuneType.LightningResistance)))
            .ForMember(d => d.HealthRune, m => m.MapFrom(s => CalculateRuneValue(s.Items, RuneType.HealthBonus)))
            .ForMember(d => d.Companions, m => m.MapFrom(s => new List<SFToolsCompanion>() { s.Companions.Bert, s.Companions.Mark, s.Companions.Kunigunde }))
            ;

        CreateMap<SFToolsCompanion, RawCompanion>()
            .ForMember(d => d.HealthRune, m => m.MapFrom(s => CalculateRuneValue(s.Items, RuneType.HealthBonus)))
            .ForMember(d => d.FireResistance, m => m.MapFrom(s => CalculateRuneValue(s.Items, RuneType.FireResistance)))
            .ForMember(d => d.ColdResistance, m => m.MapFrom(s => CalculateRuneValue(s.Items, RuneType.ColdResistance)))
            .ForMember(d => d.LightningResistance, m => m.MapFrom(s => CalculateRuneValue(s.Items, RuneType.LightningResistance)))
            .ForMember(d => d.Reaction, m => m.MapFrom(s => s.Items.Hand.Enchantment == WitchScrollType.Reaction ? 1 : 0))
            .ForMember(d => d.HasWeaponScroll, m => m.MapFrom(s => s.Items.Wpn1.Enchantment == WitchScrollType.Crit | s.Items.Wpn2.Enchantment == WitchScrollType.Crit))
            .ForMember(d => d.Strength, m => m.MapFrom(s => s.Strength.Total))
            .ForMember(d => d.Dexterity, m => m.MapFrom(s => s.Dexterity.Total))
            .ForMember(d => d.Intelligence, m => m.MapFrom(s => s.Intelligence.Total))
            .ForMember(d => d.Constitution, m => m.MapFrom(s => s.Constitution.Total))
            .ForMember(d => d.Luck, m => m.MapFrom(s => s.Luck.Total))
            .ForMember(d => d.FirstWeapon, m => m.MapFrom(s => s.Items.Wpn1))
            .ForMember(d => d.SecondWeapon, m => m.MapFrom(s => s.Items.Wpn2))

            ;

        CreateMap<SFToolsWeapon, EquipmentWeapon>()
            .ForMember(d => d.MinDmg, m => m.MapFrom(s => s.DamageMin))
            .ForMember(d => d.MaxDmg, m => m.MapFrom(s => s.DamageMax))
            .ForMember(d => d.Strength, m => m.MapFrom(s => s.Strength.Value))
            .ForMember(d => d.Dexterity, m => m.MapFrom(s => s.Dexterity.Value))
            .ForMember(d => d.Intelligence, m => m.MapFrom(s => s.Intelligence.Value))
            .ForMember(d => d.Constitution, m => m.MapFrom(s => s.Constitution.Value))
            .ForMember(d => d.Luck, m => m.MapFrom(s => s.Luck.Value))
            ;

        CreateMap<SFToolsWeapon, RawWeapon>()
            .ForMember(d => d.MinDmg, m => m.MapFrom(s => s.DamageMin))
            .ForMember(d => d.MaxDmg, m => m.MapFrom(s => s.DamageMax))
            ;

        CreateMap<SFToolsItem, EquipmentItem>()
            .ForMember(d => d.Strength, m => m.MapFrom(s => s.Strength.Value))
            .ForMember(d => d.Dexterity, m => m.MapFrom(s => s.Dexterity.Value))
            .ForMember(d => d.Intelligence, m => m.MapFrom(s => s.Intelligence.Value))
            .ForMember(d => d.Constitution, m => m.MapFrom(s => s.Constitution.Value))
            .ForMember(d => d.Luck, m => m.MapFrom(s => s.Luck.Value))
            ;

        CreateMap<Dungeon, DungeonDTO>()
            .ForMember(d => d.Enemies, m => m.MapFrom(s => s.DungeonEnemies));

        CreateMap<DungeonEnemy, DungeonEnemyDTO>();
    }
}
