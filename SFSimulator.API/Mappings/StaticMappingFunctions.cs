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

        public static int CalculateRuneValue(Slots items, RuneType runeType)
        {
            if (runeType is RuneType.None or RuneType.FireDamage or RuneType.ColdDamage or RuneType.LightningDamage)
            {
                return 0;
            }
            var itemsList = ResolveItemsAsList(items);
            int runeValue;
            if (runeType is RuneType.FireResistance or RuneType.ColdResistance or RuneType.LightningResistance)
            {
                runeValue = itemsList.Where(i => i.RuneType == runeType || i.RuneType == RuneType.TotalResistance).Sum(i => i.RuneValue);
            }
            else
            {
                runeValue = itemsList.Where(i => i.RuneType == runeType).Sum(i => i.RuneValue);
            }

            return runeType switch
            {
                RuneType.GoldBonus => Math.Min(50, runeValue),
                RuneType.ExperienceBonus => Math.Min(10, runeValue),
                RuneType.FireResistance => Math.Min(75, runeValue),
                RuneType.ColdResistance => Math.Min(75, runeValue),
                RuneType.LightningResistance => Math.Min(75, runeValue),
                RuneType.EpicChance => Math.Min(50, runeValue),
                RuneType.ItemQuality => Math.Min(5, runeValue),
                RuneType.HealthBonus => Math.Min(15, runeValue),
                _ => 0
            };
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

        public static int GetQuestRuneBonus(Maria21DataDTO dto, BonusType type)
        {
            int runeBonus, runeMax;
            RuneType runeType;
            if (type == BonusType.GOLD)
            {
                runeType = RuneType.GoldBonus;
                runeBonus = dto.Runes.Gold;
                runeMax = 50;
            }
            else
            {
                runeType = RuneType.ExperienceBonus;
                runeBonus = dto.Runes.XP;
                runeMax = 10;
            }

            var dummy = dto.Inventory.Dummy;
            var props = typeof(Slots).GetProperties();

            foreach (var property in props)
            {
                var value = property.GetValue(dummy);
                if (value is SFToolsItem item)
                {
                    runeBonus += item.RuneType == runeType ? item.RuneValue : 0;
                }
            }
            runeBonus = Math.Min(runeMax, runeBonus);
            return runeBonus;
        }

        public static int GetGuildBonus(Maria21DataDTO dto, BonusType type)
        {
            if (dto.Group.Group is null)
                return 0;

            var raid = dto.Group.Group.Raid * 2;
            var guild = type == BonusType.GOLD ? dto.Group.Group.TotalTreasure : dto.Group.Group.TotalInstructor;
            return Math.Min(200, raid + guild);
        }

        public static int SumBaseStats(Maria21DataDTO dto)
        {
            var con = dto.Constitution.Base;
            return dto.Class switch
            {
                ClassType.Warrior or ClassType.BattleMage or ClassType.Berserker => con + dto.Strength.Base,
                ClassType.Mage or ClassType.Druid or ClassType.Bard or ClassType.Necromancer => con + dto.Intelligence.Base,
                ClassType.Scout or ClassType.Assassin or ClassType.DemonHunter => con + dto.Dexterity.Base,
                _ => con,
            };
        }

        public static List<SFToolsItem> ResolveItemsAsList(Slots items)
        {
            var list = new List<SFToolsItem>
            {
                items.Head,
                items.Body,
                items.Hand,
                items.Feet,
                items.Neck,
                items.Belt,
                items.Ring,
                items.Misc
            };

            return list;
        }

        public enum BonusType
        {
            GOLD,
            XP
        }
    }
}
