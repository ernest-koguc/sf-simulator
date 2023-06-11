namespace SFSimulator.Core
{
    public class Scheduler : IScheduler
    {
        private int CurrentWeek;
        private int CurrentDay;
        private Dictionary<(int Week, int Day), ScheduleDay> Schedule { get; set; } = null!;
        public Scheduler()
        {
            CurrentWeek = 1;
            CurrentDay = 1;

            SetDefaultSchedule();
        }
        public void SetStartingPoint(int week, int day)
        {
            if (day > Schedule.Keys.Max(s => s.Day) || day < Schedule.Keys.Min(s => s.Day))
                throw new ArgumentOutOfRangeException(nameof(day));
            if (week > Schedule.Keys.Max(s => s.Week) || week < Schedule.Keys.Min(s => s.Week))
                throw new ArgumentOutOfRangeException(nameof(week));

            CurrentWeek = week;
            CurrentDay = day;
        }

        public ScheduleDay GetCurrentSchedule()
        {
            if (CurrentDay > Schedule.Keys.Where(x => x.Week == CurrentWeek).Max(x => x.Day))
            {
                CurrentDay = 1;
                CurrentWeek++;
            }

            if (CurrentWeek > Schedule.Keys.Max(s=>s.Week))
            {
                CurrentWeek = 1;
            }
            
            var schedule = Schedule[(CurrentWeek, CurrentDay)];
            CurrentDay++;
            return schedule;
        }
        public void SetCustomSchedule(Dictionary<(int Week, int Day), ScheduleDay> schedule)
        {
            if (schedule == null)
                throw new ArgumentNullException(nameof(schedule));

            var weeks = schedule.Keys.Select(s=>s.Week);
            var min = weeks.Min();
            var max = weeks.Max();
            var weeksBetween = Enumerable.Range(min, max);
            if (weeksBetween.Any(s => !weeks.Contains(s)))
                throw new ArgumentException("There can not be any gaps between weeks");

            Schedule = schedule;
        }
        public void SetDefaultSchedule()
        {
            var firstWeek = new List<EventType>() { EventType.Experience, EventType.Hourglasses, EventType.EpicQuest };
            var secondWeek = new List<EventType>() { EventType.EpicShop, EventType.EpicLuck, EventType.Toilet, EventType.Forge };
            var thirdWeek = new List<EventType>() { EventType.Gold, EventType.Souls, EventType.Pets, EventType.Witch };
            var fourthWeek = new List<EventType>() { EventType.Mushroom, EventType.EpicShop, EventType.Fortress, EventType.Pets, EventType.Forge, EventType.Toilet };
            var eightWeek = new List<EventType>() { EventType.Mushroom, EventType.Experience, EventType.Gold, EventType.Fortress, EventType.Toilet, EventType.PieceworkParty };
            var twelvethWeek = new List<EventType>() { EventType.Mushroom, EventType.EpicLuck, EventType.Fortress, EventType.Toilet, EventType.LuckyDay, EventType.Forge };

            Schedule = new()
            {
                { (1,  1),  new() },
                { (1,  2),  new() },
                { (1,  3),  new() },
                { (1,  4),  new() },
                { (1,  5),  new() { Events = firstWeek }},
                { (1,  6),  new() { Events = firstWeek }},
                { (1,  7),  new() { Events = firstWeek }},

                { (2,  1),  new() },
                { (2,  2),  new() },
                { (2,  3),  new() },
                { (2,  4),  new() },
                { (2,  5),  new() { Events = secondWeek }},
                { (2,  6),  new() { Events = secondWeek }},
                { (2,  7),  new() { Events = secondWeek }},

                { (3,  1),  new() },
                { (3,  2),  new() },
                { (3,  3),  new() },
                { (3,  4),  new() },
                { (3,  5),  new() { Events = thirdWeek }},
                { (3,  6),  new() { Events = thirdWeek }},
                { (3,  7),  new() { Events = thirdWeek }},

                { (4,  1),  new() },
                { (4,  2),  new() },
                { (4,  3),  new() },
                { (4,  4),  new() },
                { (4,  5),  new() { Events = fourthWeek }},
                { (4,  6),  new() { Events = fourthWeek }},
                { (4,  7),  new() { Events = fourthWeek }},

                { (5,  1),  new() },
                { (5,  2),  new() },
                { (5,  3),  new() },
                { (5,  4),  new() },
                { (5,  5),  new() { Events = firstWeek }},
                { (5,  6),  new() { Events = firstWeek }},
                { (5,  7),  new() { Events = firstWeek }},

                { (6,  1),  new() },
                { (6,  2),  new() },
                { (6,  3),  new() },
                { (6,  4),  new() },
                { (6,  5),  new() { Events = secondWeek }},
                { (6,  6),  new() { Events = secondWeek }},
                { (6,  7),  new() { Events = secondWeek }},

                { (7,  1),  new() },
                { (7,  2),  new() },
                { (7,  3),  new() },
                { (7,  4),  new() },
                { (7,  5),  new() { Events = thirdWeek }},
                { (7,  6),  new() { Events = thirdWeek }},
                { (7,  7),  new() { Events = thirdWeek }},

                { (8,  1),  new() },
                { (8,  2),  new() },
                { (8,  3),  new() },
                { (8,  4),  new() },
                { (8,  5),  new() { Events = eightWeek }},
                { (8,  6),  new() { Events = eightWeek }},
                { (8,  7),  new() { Events = eightWeek }},

                { (9,  1),  new() },
                { (9,  2),  new() },
                { (9,  3),  new() },
                { (9,  4),  new() },
                { (9,  5),  new() { Events = firstWeek }},
                { (9,  6),  new() { Events = firstWeek }},
                { (9,  7),  new() { Events = firstWeek }},

                { (10, 1), new() },
                { (10, 2), new() },
                { (10, 3), new() },
                { (10, 4), new() },
                { (10, 5), new() { Events = secondWeek }},
                { (10, 6), new() { Events = secondWeek }},
                { (10, 7), new() { Events = secondWeek }},

                { (11, 1), new() },
                { (11, 2), new() },
                { (11, 3), new() },
                { (11, 4), new() },
                { (11, 5), new() { Events = thirdWeek }},
                { (11, 6), new() { Events = thirdWeek }},
                { (11, 7), new() { Events = thirdWeek }},

                { (12, 1), new() },
                { (12, 2), new() },
                { (12, 3), new() },
                { (12, 4), new() },
                { (12, 5), new() { Events = twelvethWeek }},
                { (12, 6), new() { Events = twelvethWeek }},
                { (12, 7), new() { Events = twelvethWeek }}
            };
        }

    }
}
