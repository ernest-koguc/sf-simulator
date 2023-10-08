using SFSimulator.Core;

namespace SFSimulator.API.Mappings
{
    public static class StaticMappingFunctions
    {

        public static Dictionary<(int Week, int Day), ScheduleDay> MapSchedule(Schedule schedule)
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

        public static List<EventType> MapEvents(List<string> events)
        {
            var list = new List<EventType>();
            foreach (var @event in events)
            {
                var eventEnum = Enum.Parse<EventType>(@event);
                list.Add(eventEnum);
            }

            return list;
        }

        public static List<ActionType> MapActions(List<string> actions)
        {
            var list = new List<ActionType>();
            foreach (var action in actions)
            {
                var actionEnum = Enum.Parse<ActionType>(action);
                list.Add(actionEnum);
            }

            return list;
        }

        public static QuestPriorityType GetQuestPriorityType(string? priority)
        {
            return priority switch
            {
                nameof(QuestPriorityType.Gold) => QuestPriorityType.Gold,
                nameof(QuestPriorityType.Experience) => QuestPriorityType.Experience,
                nameof(QuestPriorityType.Hybrid) => QuestPriorityType.Hybrid,
                _ => QuestPriorityType.Experience
            };
        }

        public static SpinAmountType GetSpinAmount(string spinAmount)
        {
            return spinAmount == nameof(SpinAmountType.Max) ? SpinAmountType.Max : SpinAmountType.OnlyFree;
        }

        public static int GetRuneBonus(Maria21DataDTO dto, BonusType type)
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
                if (value is PlayerItem item)
                {
                    runeBonus += item.RuneType == runeType ? item.RuneValue : 0;
                }
            }
            runeBonus = Math.Min(runeMax, runeBonus);
            return runeBonus;
        }
        public static int GetGuildBonus(Maria21DataDTO dto, BonusType type)
        {
            var raid = dto.Group.Group.Raid * 2;
            var guild = type == BonusType.GOLD ? dto.Group.Group.TotalTreasure : dto.Group.Group.TotalInstructor;
            return Math.Min(200, raid + guild);
        }
        public static MountType TranslateMountType(string mountType)
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
        public static string TranslateMountType(int mountType)
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
        public static int SumBaseStats(Maria21DataDTO dto)
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
