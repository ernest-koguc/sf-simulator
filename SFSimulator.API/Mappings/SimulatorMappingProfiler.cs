using AutoMapper;
using QuestSimulator.Characters;
using QuestSimulator.DTOs;
using QuestSimulator.Enums;
using QuestSimulator.Simulation;

namespace SFSimulatorAPI.Mappings
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
    }
}
