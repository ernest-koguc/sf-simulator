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
            .ForMember(d => d.XpGuildBonus, m => m.MapFrom(s => GetGuildBonus(s, BonusType.XP)));

        CreateMap<Dungeon, DungeonDTO>()
            .ForMember(d => d.Enemies, m => m.MapFrom(s => s.DungeonEnemies));

        CreateMap<DungeonEnemy, DungeonEnemyDTO>();
    }
}
