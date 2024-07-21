namespace SFSimulator.Core
{
    public class CalendarRewardProvider : ICalendarRewardProvider
    {
        private int CurrentCalendar { get; set; } = 1;
        private int CurrentDay { get; set; } = 1;
        private bool CalendarSkipping { get; set; } = false;
        private Dictionary<int, List<CalendarRewardType>> CalendarRewards = null!;
        private List<int> experienceCalendars = new() { 5, 8, 10, 12 };

        public CalendarRewardProvider()
        {
            ConfigureCalendar(1, 1, false);
        }
        public void ConfigureCalendar(int calendar, int day, bool skipCalendar)
        {
            CalendarSkipping = skipCalendar;
            CalendarRewards = SetCalendarRewards();

            if (calendar < 1 || calendar > CalendarRewards.Keys.Max())
                throw new ArgumentOutOfRangeException(nameof(calendar));
            if (day < 1 || day > CalendarRewards[calendar].Count)
                throw new ArgumentOutOfRangeException(nameof(day));

            CurrentCalendar = calendar;
            CurrentDay = day;
        }

        public CalendarRewardType GetNextReward()
        {
            if (CurrentDay > CalendarRewards[CurrentCalendar].Count())
            {
                CurrentDay = 1;
                CurrentCalendar = CurrentCalendar == CalendarRewards.Keys.Max() ? 1 : ++CurrentCalendar;
            }

            if (CalendarSkipping && CurrentDay == 1 && !experienceCalendars.Contains(CurrentCalendar))
            {
                CurrentCalendar = CurrentCalendar == CalendarRewards.Keys.Max() ? CalendarRewards.Keys.Min() : ++CurrentCalendar;
                return CalendarRewardType.SKIP;
            }

            var calendar = CalendarRewards[CurrentCalendar];
            var reward = calendar[CurrentDay - 1];

            CurrentDay++;

            return reward;
        }

        private Dictionary<int, List<CalendarRewardType>> SetCalendarRewards()
        {
            var rewards = new Dictionary<int, List<CalendarRewardType>>()
            {
                { 1, new List<CalendarRewardType>() {
                    CalendarRewardType.STRENGTH_ATTRIBUTE,
                    CalendarRewardType.LIGHT_FRUIT,
                    CalendarRewardType.DEXTERITY_POTION,
                    CalendarRewardType.ONE_SPLINTER,
                    CalendarRewardType.LUCK_POTION,
                    CalendarRewardType.STRENGTH_POTION,
                    CalendarRewardType.ONE_GOLDBAR,
                    CalendarRewardType.ONE_RUNE,
                    CalendarRewardType.ONE_SOUL,
                    CalendarRewardType.TWO_SOUL,
                    CalendarRewardType.ETERNAL_LIFE_POTION,
                    CalendarRewardType.ONE_STONE,
                    CalendarRewardType.TWO_STONES,
                    CalendarRewardType.TWO_SPLINTERS,
                    CalendarRewardType.INTELIGENCE_POTION,
                    CalendarRewardType.THREE_SPLINTERS,
                    CalendarRewardType.ETERNAL_LIFE_POTION,
                    CalendarRewardType.THREE_GOLDBARS,
                    CalendarRewardType.CONSTITUTION_POTION,
                    CalendarRewardType.LEVEL_UP,
                }},
                { 2, new List<CalendarRewardType>() {
                    CalendarRewardType.STRENGTH_ATTRIBUTE,
                    CalendarRewardType.HOURGLASSES,
                    CalendarRewardType.ONE_WOOD,
                    CalendarRewardType.DARK_FRUIT,
                    CalendarRewardType.ONE_BOOK,
                    CalendarRewardType.HOURGLASSES,
                    CalendarRewardType.ETERNAL_LIFE_POTION,
                    CalendarRewardType.ONE_STONE,
                    CalendarRewardType.TWO_WOODS,
                    CalendarRewardType.LUCK_POTION,
                    CalendarRewardType.STRENGTH_POTION,
                    CalendarRewardType.ONE_GOLDBAR,
                    CalendarRewardType.THREE_WOODS,
                    CalendarRewardType.ONE_SPLINTER,
                    CalendarRewardType.INTELIGENCE_POTION,
                    CalendarRewardType.ONE_SOUL,
                    CalendarRewardType.HOURGLASSES,
                    CalendarRewardType.ONE_RUNE,
                    CalendarRewardType.TWO_SOUL,
                    CalendarRewardType.LEVEL_UP,
                }},
                { 3, new List<CalendarRewardType>() {
                    CalendarRewardType.LIGHT_FRUIT,
                    CalendarRewardType.ONE_STONE,
                    CalendarRewardType.INTELIGENCE_ATTRIBUTE,
                    CalendarRewardType.ETERNAL_LIFE_POTION,
                    CalendarRewardType.ONE_RUNE,
                    CalendarRewardType.ONE_MUSHROOM,
                    CalendarRewardType.TWO_STONES,
                    CalendarRewardType.DEXTERITY_POTION,
                    CalendarRewardType.THREE_STONES,
                    CalendarRewardType.LUCK_POTION,
                    CalendarRewardType.ONE_SOUL,
                    CalendarRewardType.ONE_GOLDBAR,
                    CalendarRewardType.CONSTITUTION_POTION,
                    CalendarRewardType.TWO_RUNES,
                    CalendarRewardType.INTELIGENCE_POTION,
                    CalendarRewardType.THREE_RUNES,
                    CalendarRewardType.ETERNAL_LIFE_POTION,
                    CalendarRewardType.TWO_MUSHROOMS,
                    CalendarRewardType.THREE_GOLDBARS,
                    CalendarRewardType.LEVEL_UP,
                }},
                { 4, new List<CalendarRewardType>() {
                    CalendarRewardType.HOURGLASSES,
                    CalendarRewardType.FIRE_FRUIT,
                    CalendarRewardType.LUCK_ATTRIBUTE,
                    CalendarRewardType.ONE_SOUL,
                    CalendarRewardType.DEXTERITY_POTION,
                    CalendarRewardType.STRENGTH_POTION,
                    CalendarRewardType.LUCK_POTION,
                    CalendarRewardType.ONE_RUNE,
                    CalendarRewardType.HOURGLASSES,
                    CalendarRewardType.INTELIGENCE_POTION,
                    CalendarRewardType.ONE_GOLDBAR,
                    CalendarRewardType.ONE_SPLINTER,
                    CalendarRewardType.CONSTITUTION_POTION,
                    CalendarRewardType.HOURGLASSES,
                    CalendarRewardType.TWO_SOUL,
                    CalendarRewardType.ONE_STONE,
                    CalendarRewardType.ETERNAL_LIFE_POTION,
                    CalendarRewardType.ONE_MUSHROOM,
                    CalendarRewardType.THREE_SOUL,
                    CalendarRewardType.LEVEL_UP,
                }},
                { 5, new List<CalendarRewardType>() {
                    CalendarRewardType.DEXTERITY_ATTRIBUTE,
                    CalendarRewardType.ONE_RUNE,
                    CalendarRewardType.ONE_BOOK,
                    CalendarRewardType.DEXTERITY_POTION,
                    CalendarRewardType.ONE_STONE,
                    CalendarRewardType.ONE_MUSHROOM,
                    CalendarRewardType.TWO_BOOKS,
                    CalendarRewardType.ONE_GOLDBAR,
                    CalendarRewardType.STRENGTH_POTION,
                    CalendarRewardType.EARTH_FRUIT,
                    CalendarRewardType.ONE_SOUL,
                    CalendarRewardType.ETERNAL_LIFE_POTION,
                    CalendarRewardType.ONE_SPLINTER,
                    CalendarRewardType.TWO_SOUL,
                    CalendarRewardType.TWO_RUNES,
                    CalendarRewardType.ETERNAL_LIFE_POTION,
                    CalendarRewardType.TWO_STONES,
                    CalendarRewardType.TWO_MUSHROOMS,
                    CalendarRewardType.THREE_BOOKS,
                    CalendarRewardType.LEVEL_UP,
                }},
                { 6, new List<CalendarRewardType>() {
                    CalendarRewardType.INTELIGENCE_ATTRIBUTE,
                    CalendarRewardType.ONE_SOUL,
                    CalendarRewardType.ETERNAL_LIFE_POTION,
                    CalendarRewardType.ETERNAL_LIFE_POTION,
                    CalendarRewardType.ONE_WOOD,
                    CalendarRewardType.DARK_FRUIT,
                    CalendarRewardType.TWO_WOODS,
                    CalendarRewardType.DEXTERITY_POTION,
                    CalendarRewardType.ONE_GOLDBAR,
                    CalendarRewardType.ONE_STONE,
                    CalendarRewardType.INTELIGENCE_POTION,
                    CalendarRewardType.TWO_SOUL,
                    CalendarRewardType.THREE_SOUL,
                    CalendarRewardType.ONE_MUSHROOM,
                    CalendarRewardType.TWO_STONES,
                    CalendarRewardType.ETERNAL_LIFE_POTION,
                    CalendarRewardType.THREE_WOODS,
                    CalendarRewardType.ONE_RUNE,
                    CalendarRewardType.THREE_GOLDBARS,
                    CalendarRewardType.LEVEL_UP,
                }},
                { 7, new List<CalendarRewardType>() {
                    CalendarRewardType.ONE_WOOD,
                    CalendarRewardType.INTELIGENCE_POTION,
                    CalendarRewardType.LUCK_POTION,
                    CalendarRewardType.CONSTITUTION_POTION,
                    CalendarRewardType.LUCK_ATTRIBUTE,
                    CalendarRewardType.DEXTERITY_POTION,
                    CalendarRewardType.ONE_RUNE,
                    CalendarRewardType.WATER_FRUIT,
                    CalendarRewardType.ONE_SPLINTER,
                    CalendarRewardType.TWO_SPLINTERS,
                    CalendarRewardType.ONE_GOLDBAR,
                    CalendarRewardType.THREE_GOLDBARS,
                    CalendarRewardType.ONE_SOUL,
                    CalendarRewardType.TWO_SOUL,
                    CalendarRewardType.HOURGLASSES,
                    CalendarRewardType.HOURGLASSES,
                    CalendarRewardType.STRENGTH_POTION,
                    CalendarRewardType.ETERNAL_LIFE_POTION,
                    CalendarRewardType.HOURGLASSES,
                    CalendarRewardType.LEVEL_UP,
                }},
                { 8, new List<CalendarRewardType>() {
                    CalendarRewardType.STRENGTH_POTION,
                    CalendarRewardType.ONE_RUNE,
                    CalendarRewardType.ONE_SOUL,
                    CalendarRewardType.TWO_RUNES,
                    CalendarRewardType.THREE_RUNES,
                    CalendarRewardType.LUCK_ATTRIBUTE,
                    CalendarRewardType.DEXTERITY_POTION,
                    CalendarRewardType.FIRE_FRUIT,
                    CalendarRewardType.ONE_MUSHROOM,
                    CalendarRewardType.INTELIGENCE_POTION,
                    CalendarRewardType.LUCK_POTION,
                    CalendarRewardType.HOURGLASSES,
                    CalendarRewardType.ONE_STONE,
                    CalendarRewardType.TWO_SOUL,
                    CalendarRewardType.ETERNAL_LIFE_POTION,
                    CalendarRewardType.ONE_BOOK,
                    CalendarRewardType.TWO_BOOKS,
                    CalendarRewardType.HOURGLASSES,
                    CalendarRewardType.THREE_SOUL,
                    CalendarRewardType.LEVEL_UP,
                }},
                { 9, new List<CalendarRewardType>() {
                    CalendarRewardType.CONSTITUTION_ATTRIBUTE,
                    CalendarRewardType.DEXTERITY_POTION,
                    CalendarRewardType.WATER_FRUIT,
                    CalendarRewardType.ONE_SOUL,
                    CalendarRewardType.ONE_GOLDBAR,
                    CalendarRewardType.ONE_STONE,
                    CalendarRewardType.ONE_SPLINTER,
                    CalendarRewardType.ONE_WOOD,
                    CalendarRewardType.ETERNAL_LIFE_POTION,
                    CalendarRewardType.THREE_GOLDBARS,
                    CalendarRewardType.TWO_WOODS,
                    CalendarRewardType.INTELIGENCE_POTION,
                    CalendarRewardType.STRENGTH_ATTRIBUTE,
                    CalendarRewardType.THREE_WOODS,
                    CalendarRewardType.ONE_MUSHROOM,
                    CalendarRewardType.TWO_STONES,
                    CalendarRewardType.TWO_SPLINTERS,
                    CalendarRewardType.THREE_STONES,
                    CalendarRewardType.ETERNAL_LIFE_POTION,
                    CalendarRewardType.LEVEL_UP,
                }},
                { 10, new List<CalendarRewardType>() {
                    CalendarRewardType.DEXTERITY_ATTRIBUTE,
                    CalendarRewardType.DARK_FRUIT,
                    CalendarRewardType.ONE_STONE,
                    CalendarRewardType.ONE_BOOK,
                    CalendarRewardType.ONE_WOOD,
                    CalendarRewardType.LUCK_POTION,
                    CalendarRewardType.TWO_STONES,
                    CalendarRewardType.TWO_BOOKS,
                    CalendarRewardType.DEXTERITY_POTION,
                    CalendarRewardType.ONE_GOLDBAR,
                    CalendarRewardType.HOURGLASSES,
                    CalendarRewardType.THREE_STONES,
                    CalendarRewardType.TWO_WOODS,
                    CalendarRewardType.THREE_WOODS,
                    CalendarRewardType.ETERNAL_LIFE_POTION,
                    CalendarRewardType.CONSTITUTION_POTION,
                    CalendarRewardType.INTELIGENCE_POTION,
                    CalendarRewardType.HOURGLASSES,
                    CalendarRewardType.ONE_SOUL,
                    CalendarRewardType.LEVEL_UP,
                }},
                { 11, new List<CalendarRewardType>() {
                    CalendarRewardType.DARK_FRUIT,
                    CalendarRewardType.CONSTITUTION_ATTRIBUTE,
                    CalendarRewardType.DEXTERITY_POTION,
                    CalendarRewardType.HOURGLASSES,
                    CalendarRewardType.ONE_RUNE,
                    CalendarRewardType.STRENGTH_POTION,
                    CalendarRewardType.INTELIGENCE_POTION,
                    CalendarRewardType.ONE_WOOD,
                    CalendarRewardType.ONE_GOLDBAR,
                    CalendarRewardType.ONE_STONE,
                    CalendarRewardType.ONE_MUSHROOM,
                    CalendarRewardType.ONE_SPLINTER,
                    CalendarRewardType.TWO_SPLINTERS,
                    CalendarRewardType.LUCK_POTION,
                    CalendarRewardType.TWO_MUSHROOMS,
                    CalendarRewardType.ONE_SOUL,
                    CalendarRewardType.THREE_MUSHROOMS,
                    CalendarRewardType.CONSTITUTION_ATTRIBUTE,
                    CalendarRewardType.TWO_STONES,
                    CalendarRewardType.LEVEL_UP,
                }},
                { 12, new List<CalendarRewardType>() {
                    CalendarRewardType.STRENGTH_POTION,
                    CalendarRewardType.INTELIGENCE_POTION,
                    CalendarRewardType.LIGHT_FRUIT,
                    CalendarRewardType.LUCK_POTION,
                    CalendarRewardType.ONE_SPLINTER,
                    CalendarRewardType.INTELIGENCE_ATTRIBUTE,
                    CalendarRewardType.TWO_SPLINTERS,
                    CalendarRewardType.ONE_BOOK,
                    CalendarRewardType.ONE_RUNE,
                    CalendarRewardType.HOURGLASSES,
                    CalendarRewardType.TWO_RUNES,
                    CalendarRewardType.ONE_STONE,
                    CalendarRewardType.ONE_GOLDBAR,
                    CalendarRewardType.ONE_MUSHROOM,
                    CalendarRewardType.THREE_SPLINTERS,
                    CalendarRewardType.TWO_MUSHROOMS,
                    CalendarRewardType.TWO_BOOKS,
                    CalendarRewardType.ONE_SOUL,
                    CalendarRewardType.THREE_BOOKS,
                    CalendarRewardType.LEVEL_UP,
                }},
            };
            return rewards;
        }
    }
}