namespace SFSimulator.Core;

public class SimulatedGains
{
    public required int DayIndex { get; set; }
    public int Level { get; set; }
    public long BaseStat { get; set; }

    public Dictionary<GainSource, long> ExperienceGain { get; set; } = new()
    {
        { GainSource.Expedition,  0 },
        { GainSource.Dungeon,     0 },
        { GainSource.Academy,     0 },
        { GainSource.TimeMachine, 0 },
        { GainSource.DailyTasks,  0 },
        { GainSource.WeeklyTasks, 0 },
        { GainSource.Wheel,       0 },
        { GainSource.Calendar,    0 },
        { GainSource.GuildFight,  0 },
        { GainSource.Pets,        0 },
        { GainSource.Arena,       0 },
        { GainSource.Total,       0 }
    };

    public Dictionary<GainSource, decimal> BaseStatGain { get; set; } = new()
    {
        { GainSource.Expedition,  0 },
        { GainSource.Dungeon,     0 },
        { GainSource.GoldPit,     0 },
        { GainSource.TimeMachine, 0 },
        { GainSource.DailyTasks,  0 },
        { GainSource.WeeklyTasks, 0 },
        { GainSource.Wheel,       0 },
        { GainSource.Calendar,    0 },
        { GainSource.Guard,       0 },
        { GainSource.Gem,         0 },
        { GainSource.Item,        0 },
        { GainSource.DiceGame,    0 },
        { GainSource.Pets,        0 },
        { GainSource.Arena,       0 },
        { GainSource.Total,       0 }
    };
}