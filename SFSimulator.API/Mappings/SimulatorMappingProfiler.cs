using AutoMapper;
using SFSimulator.Core;
using static SFSimulator.API.Mappings.StaticMappingFunctions;

namespace SFSimulator.API.Mappings;

public class SimulatorMappingProfiler : Profile
{
    public SimulatorMappingProfiler()
    {
        CreateMap<SimulationOptionsDTO, Character>();

        CreateMap<SimulateDungeonCharacterDTO, Character>();

        CreateMap<Maria21DataDTO, EndpointDataDTO>()
            .ForMember(d => d.CharacterName, m => m.MapFrom(s => s.Name + "@" + s.Prefix.Replace(" ", string.Empty)))
            .ForMember(d => d.ScrapbookFillness, m => m.MapFrom(s => Math.Round(s.Book / 2283f * 100, 3)))
            .ForMember(d => d.Level, m => m.MapFrom(s => s.Level))
            .ForMember(d => d.Experience, m => m.MapFrom(s => s.XP))
            .ForMember(d => d.GoldPitLevel, m => m.MapFrom(s => s.Underworld.GoldPit))
            .ForMember(d => d.AcademyLevel, m => m.MapFrom(s => s.Fortress.Academy))
            .ForMember(d => d.GemMineLevel, m => m.MapFrom(s => s.Fortress.GemMine))
            .ForMember(d => d.TreasuryLevel, m => m.MapFrom(s => s.Fortress.Treasury))
            .ForMember(d => d.Tower, m => m.MapFrom(s => Math.Max(s.Dungeons.Tower, 0)))
            .ForMember(d => d.XpRuneBonus, m => m.MapFrom(s => GetRuneBonus(s, BonusType.XP)))
            .ForMember(d => d.GoldRuneBonus, m => m.MapFrom(s => GetRuneBonus(s, BonusType.GOLD)))
            .ForMember(d => d.MountType, m => m.MapFrom(s => TranslateMountType(s.Mount)))
            .ForMember(d => d.HasGoldScroll, m => m.MapFrom(s => s.Items.Ring.HasEnchantment || s.Inventory.Dummy.Ring.HasEnchantment))
            .ForMember(d => d.HasExperienceScroll, m => m.MapFrom(s => s.Items.Head.HasEnchantment || s.Inventory.Dummy.Head.HasEnchantment))
            .ForMember(d => d.BaseStat, m => m.MapFrom(s => SumBaseStats(s)))
            .ForMember(d => d.HydraHeads, m => m.MapFrom(s => s.Group.Group.Hydra))
            .ForMember(d => d.GoldGuildBonus, m => m.MapFrom(s => GetGuildBonus(s, BonusType.GOLD)))
            .ForMember(d => d.XpGuildBonus, m => m.MapFrom(s => GetGuildBonus(s, BonusType.XP)))
            .ForMember(d => d.Class, m => m.MapFrom(s => (ClassType)s.Class))
            .ForMember(d => d.Strength, m => m.MapFrom(s => s.Strength.Bonus + s.Strength.Base))
            .ForMember(d => d.Dexterity, m => m.MapFrom(s => s.Dexterity.Bonus + s.Dexterity.Base))
            .ForMember(d => d.Intelligence, m => m.MapFrom(s => s.Intelligence.Bonus + s.Intelligence.Base))
            .ForMember(d => d.Constitution, m => m.MapFrom(s => s.Constitution.Bonus + s.Constitution.Base))
            .ForMember(d => d.Luck, m => m.MapFrom(s => s.Luck.Bonus + s.Luck.Base))
            .ForMember(d => d.Armor, m => m.MapFrom(s => s.Armor))
            .ForMember(d => d.FirstWeapon, m => m.MapFrom(s => s.Items.Wpn1))
            .ForMember(d => d.SecondWeapon, m => m.MapFrom(s => s.Items.Wpn2))
            .ForMember(d => d.RuneBonuses, m => m.MapFrom(s => s.Runes))
            .ForMember(d => d.HasGlovesScroll, m => m.MapFrom(s => s.Items.Hand.HasEnchantment))
            .ForMember(d => d.HasWeaponScroll, m => m.MapFrom(s => s.Items.Wpn1.HasEnchantment || s.Items.Wpn2.HasEnchantment))
            .ForMember(d => d.HasEternityPotion, m => m.MapFrom(s => s.Potions.Any(p => p.Type == 6)))
            .ForMember(d => d.GladiatorLevel, m => m.MapFrom(s => s.Fortress.Gladiator))
            .ForMember(d => d.SoloPortal, m => m.MapFrom(s => s.Dungeons.Player))
            .ForMember(d => d.GuildPortal, m => m.MapFrom(s => s.Dungeons.Group))
            .ForMember(d => d.Calendar, m => m.MapFrom(s => s.CalendarType))
            .ForMember(d => d.CalendarDay, m => m.MapFrom(s => s.CalendarDay))
        ;

        CreateMap<WeaponDto, Weapon>()
            .ForMember(d => d.MinDmg, m => m.MapFrom(s => s.DamageMin))
            .ForMember(d => d.MaxDmg, m => m.MapFrom(s => s.DamageMax))
            .ForMember(d => d.RuneBonus, m => m.MapFrom(s => s.RuneValue))
            .ForMember(d => d.DamageRuneType, m => m.MapFrom(s => GetDamageRuneType(s.RuneType)));

        CreateMap<Runes, ResistanceRuneBonuses>()
            .ForMember(d => d.HealthRune, m => m.MapFrom(s => s.Health))
            .ForMember(d => d.FireResistance, m => m.MapFrom(s => s.ResistanceFire))
            .ForMember(d => d.ColdResistance, m => m.MapFrom(s => s.ResistanceCold))
            .ForMember(d => d.LightningResistance, m => m.MapFrom(s => s.ResistanceLightning));

        CreateMap<Dungeon, DungeonDTO>()
            .ForMember(d => d.Enemies, m => m.MapFrom(s => s.DungeonEnemies));

        CreateMap<DungeonEnemy, DungeonEnemyDTO>();
    }
}
