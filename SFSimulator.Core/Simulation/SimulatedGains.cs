namespace SFSimulator.Core
{
    public class SimulatedGains
    {
        public int DayIndex { get; set; }

        public Dictionary<GainSource, long> ExperienceGain { get; set; } = new()
        {
            { GainSource.QUEST, 0         },
            { GainSource.ACADEMY, 0       },
            { GainSource.TIME_MACHINE, 0  },
            { GainSource.DAILY_TASKS, 0 },
            { GainSource.WEEKLY_TASKS, 0 },
            { GainSource.WHEEL, 0         },
            { GainSource.CALENDAR, 0      },
            { GainSource.GUILD_FIGHT, 0   },
            { GainSource.ARENA, 0         },
            { GainSource.TOTAL, 0         }
        };

        public Dictionary<GainSource, decimal> BaseStatGain { get; set; } = new()
        {
            { GainSource.QUEST, 0         },
            { GainSource.GOLD_PIT, 0      },
            { GainSource.TIME_MACHINE, 0  },
            { GainSource.DAILY_TASKS, 0 },
            { GainSource.WEEKLY_TASKS, 0 },
            { GainSource.WHEEL, 0         },
            { GainSource.CALENDAR, 0      },
            { GainSource.GUARD, 0         },
            { GainSource.GEM, 0           },
            { GainSource.ITEM, 0          },
            { GainSource.DICE_GAME, 0     },
            { GainSource.ARENA, 0         },
            { GainSource.TOTAL, 0         }
        };
    }
}
