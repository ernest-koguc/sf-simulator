using QuestSimulator.Enums;

namespace QuestSimulator.Simulation
{
    public class SimulatedDay
    {
        public int DayIndex { get; set; }

        public Dictionary<GainSource, int> ExperienceGain { get; set; } = new()
        {
            { GainSource.QUEST, 0 },
            { GainSource.ARENA, 0 },
            { GainSource.DAILY_MISSION, 0 },
            { GainSource.ACADEMY, 0 },
            { GainSource.TIME_MACHINE, 0 },
            { GainSource.WHEEL, 0 },
            { GainSource.CALENDAR, 0 },
            { GainSource.GUILD_FIGHT, 0 }
        };

        public Dictionary<GainSource, float> BaseStatGain { get; set; } = new()
        {
            { GainSource.QUEST, 0 },
            { GainSource.GOLD_PIT, 0 },
            { GainSource.TIME_MACHINE, 0 },
            { GainSource.WHEEL, 0 },
            { GainSource.CALENDAR, 0 },
            { GainSource.GUARD, 0 },
            { GainSource.GEM, 0 },
            { GainSource.ITEM, 0 },
            { GainSource.DICE_GAME, 0 }
        };
    }
}
