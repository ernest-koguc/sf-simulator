using AutoMapper;
using SFSimulator.Core;
using static SFSimulator.Frontend.MappingUtils;

namespace SFSimulator.Frontend.Mapper;

public class SimulatorMappingProfile : Profile
{
    public SimulatorMappingProfile()
    {
        CreateMap<Maria21DataDTO, ExperienceBonus>()
            .ForMember(d => d.ScrapbookFillness, m => m.MapFrom(s => Math.Round(s.Book / (decimal)CoreShared.SCRAPBOOK_LIMIT * 100, 2)))
            .ForMember(d => d.GuildBonus, m => m.MapFrom(s => GetGuildBonus(s, BonusType.XP)))
            .ForMember(d => d.RuneBonus, m => m.MapFrom(s => GetQuestRuneBonus(s, BonusType.XP)))
            .ForMember(d => d.HasExperienceScroll, m => m.MapFrom(s => s.Items.Head.Enchantment == WitchScrollType.QuestExperience || s.Inventory.Dummy.Head.Enchantment == WitchScrollType.QuestExperience))
            .ForMember(d => d.ScrapbookPlusGuildPlusRuneBonus, m => m.Ignore())
            .ForMember(d => d.CombinedBonus, m => m.Ignore())
            ;

        CreateMap<Maria21DataDTO, GoldBonus>()
            .ForMember(d => d.Tower, m => m.MapFrom(s => Math.Max(s.Dungeons.Tower, 0)))
            .ForMember(d => d.GuildBonus, m => m.MapFrom(s => GetGuildBonus(s, BonusType.GOLD)))
            .ForMember(d => d.RuneBonus, m => m.MapFrom(s => GetQuestRuneBonus(s, BonusType.GOLD)))
            .ForMember(d => d.HasGoldScroll, m => m.MapFrom(s => s.Items.Ring.Enchantment == WitchScrollType.QuestGold || s.Inventory.Dummy.Ring.Enchantment == WitchScrollType.QuestGold))
            .ForMember(d => d.HasArenaGoldScroll, m => m.MapFrom(s => s.Items.Misc.Enchantment == WitchScrollType.ArenaGold || s.Inventory.Dummy.Misc.Enchantment == WitchScrollType.ArenaGold))
            .ForMember(d => d.TowerPlusGuildBonus, m => m.Ignore())
            ;

        CreateMap<Maria21DataDTO, SimulationContext>()
            .ForMember(d => d.GoldBonus, m => m.MapFrom(s => s))
            .ForMember(d => d.ExperienceBonus, m => m.MapFrom(s => s))
            .ForMember(d => d.Level, m => m.MapFrom(s => s.Level))
            .ForMember(d => d.Experience, m => m.MapFrom(s => s.XP))
            .ForMember(d => d.GoldPitLevel, m => m.MapFrom(s => s.Underworld.GoldPit))
            .ForMember(d => d.AcademyLevel, m => m.MapFrom(s => s.Fortress.Academy))
            .ForMember(d => d.GemMineLevel, m => m.MapFrom(s => s.Fortress.GemMine))
            .ForMember(d => d.TreasuryLevel, m => m.MapFrom(s => s.Fortress.Treasury))
            .ForMember(d => d.Mount, m => m.MapFrom(s => s.Mount))
            .ForMember(d => d.BaseStat, m => m.MapFrom(s => SumBaseStats(s)))
            .ForMember(d => d.HydraHeads, m => m.MapFrom(s => s.Group.Group.Hydra))
            .ForMember(d => d.Class, m => m.MapFrom(s => s.Class))
            .ForMember(d => d.BaseStrength, m => m.MapFrom(s => s.Strength.Base))
            .ForMember(d => d.BaseDexterity, m => m.MapFrom(s => s.Dexterity.Base))
            .ForMember(d => d.BaseIntelligence, m => m.MapFrom(s => s.Intelligence.Base))
            .ForMember(d => d.BaseConstitution, m => m.MapFrom(s => s.Constitution.Base))
            .ForMember(d => d.BaseLuck, m => m.MapFrom(s => s.Luck.Base))
            .ForMember(d => d.GladiatorLevel, m => m.MapFrom(s => s.Fortress.Gladiator))
            .ForMember(d => d.SoloPortal, m => m.MapFrom(s => s.Dungeons.Player))
            .ForMember(d => d.GuildPortal, m => m.MapFrom(s => s.Dungeons.Group))
            .ForMember(d => d.Calendar, m => m.MapFrom(s => s.CalendarType))
            .ForMember(d => d.CalendarDay, m => m.MapFrom(s => s.CalendarDay == 0 ? 1 : s.CalendarDay))
            .ForMember(d => d.Items, m => m.MapFrom(s => MapItems(s.Class, s.Items)))
            .ForMember(d => d.DungeonsData, m => m.MapFrom(s => s.Dungeons))
            .ForMember(d => d.Potions, m => m.MapFrom(s => s.Potions))
            .ForMember(d => d.Aura, m => m.MapFrom(s => s.Toilet.Aura))
            // Is it guild knights or just player knights???
            .ForMember(d => d.GuildKnights, m => m.MapFrom(s => s.Fortress.Knights))
            .ForMember(d => d.GuildRaids, m => m.MapFrom(s => s.Group.Group.Raid))
            .ForMember(d => d.Companions, m => m.MapFrom(s => new List<SFToolsCompanion>() { s.Companions.Bert, s.Companions.Mark, s.Companions.Kunigunde }))
            .ForMember(d => d.Pets, m => m.MapFrom(s => new PetsState(s.Pets)))
            .AfterMap((s, d) =>
            {
                foreach (var companion in d.Companions)
                {
                    companion.Character = d;
                    companion.Class = companion.Class == ClassType.Warrior ? ClassType.Bert : companion.Class;
                }
            })
            ;

        CreateMap<SFToolsCompanion, Companion>()
            .ForMember(d => d.Class, m => m.MapFrom(s => s.Class))
            .ForMember(d => d.Items, m => m.MapFrom(s => MapItems(s.Class, s.Items)))
            ;

        CreateMap<SFToolsItem, RawWeapon>()
            .ForMember(d => d.MinDmg, m => m.MapFrom(s => s.DamageMin))
            .ForMember(d => d.MaxDmg, m => m.MapFrom(s => s.DamageMax))
            ;

        this.DisableConstructorMapping();
    }
}
