using AutoMapper;
using SFSimulator.Core;
using System.Xml;

namespace SFSimulator.API.Mappings
{
    public class SimulatorMappingProfiler : Profile
    {
        public SimulatorMappingProfiler()
        {
            CreateMap<SimulationOptionsDTO, Character>()
                .ForMember(c => c.Mount, m => m.MapFrom(s => TranslateMountType(s.MountType)));

            CreateMap<Character, CharacterDTO>();

            CreateMap<SimulationOptionsDTO, SimulationOptions>()
                .ForMember(d => d.QuestPriority, m => m.MapFrom((s, _) =>
                {
                    if (s.QuestPriority?.ToUpper() == "GOLD")
                        return Priority.GOLD;
                    if (s.QuestPriority?.ToUpper() == "EXPERIENCE")
                        return Priority.XP;
                    if (s.QuestPriority?.ToUpper() == "HYBRID")
                        return Priority.HYBRID;
                    return Priority.XP;
                }))
                .ForMember(d => d.PriorityAfterSwitch, m => m.MapFrom((s, _) =>
                {
                    if (s.PriorityAfterSwitch?.ToUpper() == "GOLD")
                        return Priority.GOLD;
                    if (s.PriorityAfterSwitch?.ToUpper() == "EXPERIENCE")
                        return Priority.XP;
                    if (s.PriorityAfterSwitch?.ToUpper() == "HYBRID")
                        return Priority.HYBRID;
                    return Priority.XP;
                }))
                .ForMember(d => d.HybridRatio, m => m.MapFrom(s => s.HybridRatio))
                .ForMember(d => d.SwitchPriority, m => m.MapFrom(s => s.SwitchPriority))
                .ForMember(d => d.SwitchLevel, m => m.MapFrom(s => s.SwitchLevel))
                .ForMember(d => d.GoldBonus, m => m.MapFrom((s, _) =>
                {
                    return new GoldBonus(s.Tower, s.GoldGuildBonus, s.GoldRuneBonus, s.HasGoldScroll);
                }))
                .ForMember(d => d.ExperienceBonus, m => m.MapFrom((s, _) =>
                {
                    return new ExperienceBonus(s.ScrapbookFillness, s.XpGuildBonus, s.XpRuneBonus, s.HasExperienceScroll);
                }))
                .ForMember(d => d.DrinkBeerOneByOne, m => m.MapFrom(s => s.DrinkBeerOneByOne))
                .ForMember(d => d.DailyThirst, m => m.MapFrom(s => s.DailyThirst));

            CreateMap<Maria21DataDTO, EndpointDataDTO>()
                .ForMember(d => d.CharacterName, m => m.MapFrom(s => s.Name + "@" + s.Prefix.Replace(" ", string.Empty)))
                .ForMember(d => d.ScrapbookFillness, m => m.MapFrom(s => Math.Round(s.Book / 2283f*100, 3)))
                .ForMember(d => d.Level, m => m.MapFrom(s => s.Level))
                .ForMember(d => d.Experience, m => m.MapFrom(s => s.XP))
                .ForMember(d => d.GoldPitLevel, m => m.MapFrom(s => s.Underworld.GoldPit))
                .ForMember(d => d.AcademyLevel, m => m.MapFrom(s => s.Fortress.Academy))
                .ForMember(d => d.GemMineLevel, m => m.MapFrom(s => s.Fortress.GemMine))
                .ForMember(d => d.TreasuryLevel, m => m.MapFrom(s => s.Fortress.Treasury))
                .ForMember(d => d.Tower, m => m.MapFrom(s => s.Dungeons.Tower))
                .ForMember(d => d.XpRuneBonus, m => m.MapFrom(s => s.Runes.XP))
                .ForMember(d => d.GoldRuneBonus, m => m.MapFrom(s => s.Runes.Gold))
                .ForMember(d => d.MountType, m => m.MapFrom(s => TranslateMountType(s.Mount)))
                .ForMember(d => d.HasGoldScroll, m => m.MapFrom(s => s.Items.Ring.Enchantment != 0))
                .ForMember(d => d.HasExperienceScroll, m => m.MapFrom(s => s.Items.Head.Enchantment != 0))
                .ForMember(d => d.BaseStat, m => m.MapFrom(s => SumBaseStats(s)))
                .ForMember(d => d.HydraHeads, m => m.MapFrom(s => s.Group.Group.Hydra))
                .ForMember(d => d.GoldGuildBonus, m => m.MapFrom(s => GetGuildBonus(s, BonusType.GOLD)))
                .ForMember(d => d.XpGuildBonus, m => m.MapFrom(s => GetGuildBonus(s, BonusType.XP)));
        }
        private int GetGuildBonus(Maria21DataDTO dto, BonusType type)
        {
            var raid = dto.Dungeons.Raid * 2;
            var guild = type == BonusType.GOLD ? dto.Group.Group.TotalTreasure : dto.Group.Group.TotalInstructor;
            return Math.Min(200, raid + guild);

        }
        private MountType TranslateMountType(string mountType)
        {
            return mountType switch
            {
                "Griffin" => MountType.Griffin,
                "Tiger" => MountType.Tiger,
                "Horse" => MountType.Horse,
                "Pig" => MountType.Pig,
                _ => MountType.None,
            };
        }
        private string TranslateMountType(int mountType)
        {
            return mountType switch
            {
                1 => "Pig",
                2 => "Horse",
                3 => "Tiger",
                4 => "Griffin",
                _ => "None",
            };
        }
        private int SumBaseStats(Maria21DataDTO dto)
        {
            var con = dto.Constitution.Base;
            return dto.Class switch
            {
                1 or 5 or 6 => con + dto.Strength.Base,
                2 or 8 or 9 => con + dto.Intelligence.Base,
                3 or 4 or 7 => con + dto.Dexterity.Base,
                _ => con,
            };
        }
        public enum BonusType
        {
            GOLD,
            XP
        }
    }
}
