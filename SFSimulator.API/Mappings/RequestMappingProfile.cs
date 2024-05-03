using AutoMapper;
using SFSimulator.API.Requests;
using SFSimulator.Core;
using static SFSimulator.API.Mappings.StaticMappingFunctions;

namespace SFSimulator.API.Mappings;

public class RequestMappingProfile : Profile
{
    public RequestMappingProfile()
    {
        CreateMap<SimulateRequest, Character>()
            ;

        CreateMap<SimulateRequest, SimulationOptions>()
            .ForMember(d => d.SwitchPriority, m => m.MapFrom(s => s.SwitchPriority))
            .ForMember(d => d.SwitchLevel, m => m.MapFrom(s => s.SwitchLevel))
            .ForMember(d => d.FightsForGold, m => m.MapFrom(s => s.FightsForGold))
            .ForMember(d => d.GoldBonus, m => m.MapFrom((s, _) =>
            {
                return new GoldBonus(s.Tower, s.GoldGuildBonus, s.GoldRuneBonus, s.HasGoldScroll, s.HasArenaGoldScroll);
            }))
            .ForMember(d => d.ExperienceBonus, m => m.MapFrom((s, _) =>
            {
                return new ExperienceBonus(s.ScrapbookFillness, s.XpGuildBonus, s.XpRuneBonus, s.HasExperienceScroll);
            }))
            .ForMember(d => d.DrinkBeerOneByOne, m => m.MapFrom(s => s.DrinkBeerOneByOne))
            .ForMember(d => d.DailyThirst, m => m.MapFrom(s => s.DailyThirst))
            .ForMember(d => d.Schedule, m => m.MapFrom(s => MapSchedule(s.Schedule)))
            .ForMember(d => d.WeeklyTasksOptions, m => m.MapFrom(s => new WeeklyTasksOptions(s.DoWeeklyTasks, s.DrinkExtraWeeklyBeer)))
            .ForMember(d => d.ExpeditionOptions, m => m.MapFrom(s => s.ExpeditionOptions))
            .ForMember(d => d.ExpeditionOptionsAfterSwitch, m => m.MapFrom(s => s.ExpeditionOptionsAfterSwitch))
            .ForMember(d => d.QuestOptions, m => m.MapFrom(s => s.QuestOptions))
            .ForMember(d => d.QuestOptionsAfterSwitch, m => m.MapFrom(s => s.QuestOptionsAfterSwitch))
            .ForMember(d => d.ExpeditionsInsteadOfQuests, m => m.MapFrom(s => s.ExpeditionsInsteadOfQuests))
            .ForMember(d => d.Mount, m => m.MapFrom(s => s.MountType))
            ;


        CreateMap<SimulateDungeonRequest, RawFightable>()
            .ForMember(d => d.GuildPortal, m => m.MapFrom(s => s.GuildPortal / 100D))
            .ForMember(d => d.SoloPortal, m => m.MapFrom(s => s.SoloPortal / 100D))
            .ForMember(d => d.Companions, m => m.MapFrom(s => s.Companions))
            ;
    }
}
