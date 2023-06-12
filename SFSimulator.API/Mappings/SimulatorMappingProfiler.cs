using AutoMapper;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.Extensions.Logging;
using SFSimulator.Core;

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

            CreateMap<Maria21DataDTO, EndpointDataDTO>()
                .ForMember(d => d.CharacterName, m => m.MapFrom(s => s.Name + "@" + s.Prefix.Replace(" ", string.Empty)))
                .ForMember(d => d.ScrapbookFillness, m => m.MapFrom(s => Math.Round(s.Book / 2283f*100, 3)))
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
        }

        private Dictionary<(int Week, int Day), ScheduleDay> MapSchedule(Schedule schedule)
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

        private List<EventType> MapEvents(List<string> events)
        {
            var list = new List<EventType>();
            foreach (var @event in events)
            {
                var eventEnum = Enum.Parse<EventType>(@event);
                list.Add(eventEnum);
            }

            return list;
        }

        private List<ActionType> MapActions(List<string> actions)
        {
            var list = new List<ActionType>();
            foreach (var action in actions)
            {
                var actionEnum = Enum.Parse<ActionType>(action);
                list.Add(actionEnum);
            }

            return list;
        }

        private QuestPriorityType GetQuestPriorityType(string? priority)
        {
            return priority switch
            {
                nameof(QuestPriorityType.Gold) => QuestPriorityType.Gold,
                nameof(QuestPriorityType.Experience) => QuestPriorityType.Experience,
                nameof(QuestPriorityType.Hybrid) => QuestPriorityType.Hybrid,
                _ => QuestPriorityType.Experience
            };
        }

        private SpinAmountType GetSpinAmount(string spinAmount)
        {
            return spinAmount == nameof(SpinAmountType.Max) ? SpinAmountType.Max : SpinAmountType.OnlyFree;
        }

        private int GetRuneBonus(Maria21DataDTO dto, BonusType type)
        {
            int runeType, runeBonus, runeMax;
            if (type == BonusType.GOLD)
            {
                runeType = 1;
                runeBonus = dto.Runes.Gold;
                runeMax = 50;
            }
            else
            {
                runeType = 4;
                runeBonus = dto.Runes.XP;
                runeMax = 10;
            }

            var dummy = dto.Inventory.Dummy;
            var props = typeof(Slots).GetProperties();

            foreach (var property in props)
            {
                var value = property.GetValue(dummy);
                if (value is PlayerItem)
                {
                    var item = (PlayerItem)value;
                    runeBonus += item.RuneType == runeType ? item.RuneValue : 0;
                }
            }
            runeBonus = Math.Min(runeMax, runeBonus);
            return runeBonus;
        }
        private int GetGuildBonus(Maria21DataDTO dto, BonusType type)
        {
            var raid = dto.Group.Group.Raid * 2;
            var guild = type == BonusType.GOLD ? dto.Group.Group.TotalTreasure : dto.Group.Group.TotalInstructor;
            return Math.Min(200, raid + guild);
        }
        private MountType TranslateMountType(string mountType)
        {
            return mountType switch
            {
                nameof(MountType.Griffin) => MountType.Griffin,
                nameof(MountType.Tiger) => MountType.Tiger,
                nameof(MountType.Horse) => MountType.Horse,
                nameof(MountType.Pig) => MountType.Pig,
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
