using QuestSimulator.Enums;

namespace QuestSimulator.TavernEvents
{
    public class EventScheduler : IEventScheduler
    {
        private int startWeek;
        private int startDay;
        private readonly Dictionary<int, List<EventType>> EventWeeks;
        public EventScheduler()
        {
            startWeek = 1;
            startDay = 1;

            EventWeeks = SetEventWeeks();
        }
        public void SetEvent(int week, int day)
        {
            if (day < 1 || day > 7)
                throw new ArgumentOutOfRangeException(nameof(day));
            if (week < 1 || week > 12)
                throw new ArgumentOutOfRangeException(nameof(week));

            startWeek = week;
            startDay = day;
        }

        public List<EventType> GetCurrentEvents(int currentDay)
        {
            var day = (currentDay + startDay - 1) % 7;
            if (day == 0)
                day = 7;

            if (day < 5)
                return new List<EventType>();

            var week = (int)Math.Floor((startDay + currentDay - 2) / 7d) + startWeek;

            if (week > 12)
                week %= 12;
            if (week==0)
                week = 12;

            return EventWeeks[week];
        }
        private Dictionary<int, List<EventType>> SetEventWeeks()
        {
            var weeks = new Dictionary<int, List<EventType>>()
            {
                { 1, new List<EventType>()
                {
                    EventType.EXPERIENCE,
                    EventType.HOURGLASSES,
                    EventType.EPIC_QUEST
                }},
                { 2, new List<EventType>()
                {
                    EventType.EPIC_SHOP,
                    EventType.EPIC_LUCK,
                    EventType.TOILET,
                    EventType.FORGE
                }},
                { 3, new List<EventType>()
                {
                    EventType.GOLD,
                    EventType.SOULS,
                    EventType.PETS,
                    EventType.WITCH
                }},
                { 4, new List<EventType>()
                {
                    EventType.MUSHROOM,
                    EventType.EPIC_SHOP,
                    EventType.FORTRESS,
                    EventType.PETS,
                    EventType.FORGE,
                    EventType.TOILET
                }},
                { 5, new List<EventType>()
                {
                    EventType.EXPERIENCE,
                    EventType.HOURGLASSES,
                    EventType.EPIC_QUEST,
                }},
                { 6, new List<EventType>()
                {
                    EventType.EPIC_SHOP,
                    EventType.EPIC_LUCK,
                    EventType.TOILET,
                    EventType.FORGE
                }},
                { 7, new List<EventType>()
                {
                    EventType.GOLD,
                    EventType.SOULS,
                    EventType.PETS,
                    EventType.WITCH
                }},
                { 8, new List<EventType>()
                {
                    EventType.MUSHROOM,
                    EventType.EXPERIENCE,
                    EventType.GOLD,
                    EventType.FORTRESS,
                    EventType.TOILET,
                    EventType.PIECEWORK_PARTY
                }},
                { 9, new List<EventType>()
                {
                    EventType.EXPERIENCE,
                    EventType.HOURGLASSES,
                    EventType.EPIC_QUEST
                }},
                { 10, new List<EventType>()
                {
                    EventType.EPIC_SHOP,
                    EventType.EPIC_LUCK,
                    EventType.TOILET,
                    EventType.FORGE
                }},
                { 11, new List<EventType>()
                {
                    EventType.GOLD,
                    EventType.SOULS,
                    EventType.PETS,
                    EventType.WITCH
                }},
                { 12, new List<EventType>()
                {
                    EventType.MUSHROOM,
                    EventType.EPIC_LUCK,
                    EventType.FORTRESS,
                    EventType.TOILET,
                    EventType.LUCKY_DAY,
                    EventType.FORGE
                }}
            };
            return weeks;
        }

    }
}
