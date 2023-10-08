using AutoMapper;
using SFSimulator.API.Requests;
using SFSimulator.Core;
using static SFSimulator.API.Mappings.StaticMappingFunctions;

namespace SFSimulator.API.Mappings;

public class RequestMappingProfile : Profile
{
    public RequestMappingProfile()
    {
        CreateMap<SimulateUntilLevelRequest, Character>()
            .ForMember(d => d.Level, m => m.MapFrom(s => s.Level));

        CreateMap<SimulateDaysRequest, Character>();

        CreateMap<SimulateUntilLevelRequest, SimulationOptions>()
            .ForMember(c => c.Mount, m => m.MapFrom(s => TranslateMountType(s.MountType)))
            .ForMember(d => d.QuestPriority, m => m.MapFrom(s => GetQuestPriorityType(s.QuestPriority)))
            .ForMember(d => d.PriorityAfterSwitch, m => m.MapFrom(s => GetQuestPriorityType(s.PriorityAfterSwitch)))
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
            .ForMember(d => d.DailyThirst, m => m.MapFrom(s => s.DailyThirst))
            .ForMember(d => d.SpinAmount, m => m.MapFrom(s => GetSpinAmount(s.SpinAmount)))
            .ForMember(d => d.Schedule, m => m.MapFrom(s => MapSchedule(s.Schedule)));

        CreateMap<SimulateDaysRequest, SimulationOptions>()
            .ForMember(c => c.Mount, m => m.MapFrom(s => TranslateMountType(s.MountType)))
            .ForMember(d => d.QuestPriority, m => m.MapFrom(s => GetQuestPriorityType(s.QuestPriority)))
            .ForMember(d => d.PriorityAfterSwitch, m => m.MapFrom(s => GetQuestPriorityType(s.PriorityAfterSwitch)))
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
            .ForMember(d => d.DailyThirst, m => m.MapFrom(s => s.DailyThirst))
            .ForMember(d => d.SpinAmount, m => m.MapFrom(s => GetSpinAmount(s.SpinAmount)))
            .ForMember(d => d.Schedule, m => m.MapFrom(s => MapSchedule(s.Schedule)));

        CreateMap<SimulateDungeonRequest, Character>();
    }
}
