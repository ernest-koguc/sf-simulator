namespace SFSimulator.Core;

public static class TypesMapper
{
    public static IEnumerable<TypeMap> Types() =>
    [
        new TypeMap(typeof(GameLogic), typeof(IGameLogic), TypeMapOption.None),

        new TypeMap(typeof(ItemValueProvider), typeof(IItemValueProvider), TypeMapOption.None),

        new TypeMap(typeof(GameSimulator), typeof(IGameSimulator), TypeMapOption.None),
        new TypeMap(typeof(CalendarRewardProvider), typeof(ICalendarRewardProvider), TypeMapOption.None),
        new TypeMap(typeof(Scheduler), typeof(IScheduler), TypeMapOption.None),


        new TypeMap(typeof(QuestHelper), typeof(IQuestHelper), TypeMapOption.None),
        new TypeMap(typeof(QuestFactory), typeof(IQuestFactory), TypeMapOption.None),

        new TypeMap(typeof(ThirstSimulator), typeof(IThirstSimulator), TypeMapOption.None),

        new TypeMap(typeof(Curves), typeof(ICurves), TypeMapOption.None),
        new TypeMap(typeof(Random), typeof(Random), TypeMapOption.None),

        new TypeMap(typeof(ItemGenerator), typeof(IItemGenerator), TypeMapOption.None),

        new TypeMap(typeof(DungeonSimulator), typeof(IDungeonSimulator), TypeMapOption.None),
        new TypeMap(typeof(DungeonProvider), typeof(IDungeonProvider), TypeMapOption.None),
        new TypeMap(typeof(FightableContextFactory), typeof(IFightableContextFactory), TypeMapOption.None),
        new TypeMap(typeof(CritChanceProvider), typeof(ICritChanceProvider), TypeMapOption.None),
        new TypeMap(typeof(DamageProvider), typeof(IDamageProvider), TypeMapOption.None),
        new TypeMap(typeof(BonusMelodyLengthProvider), typeof(IBonusMelodyLengthProvider), TypeMapOption.None),
        new TypeMap(typeof(WeeklyTasksRewardProvider), typeof(IWeeklyTasksRewardProvider), TypeMapOption.None),
        new TypeMap(typeof(ExpeditionService), typeof(IExpeditionService), TypeMapOption.None),
        new TypeMap(typeof(BlackSmithAdvisor), typeof(IBlackSmithAdvisor), TypeMapOption.None),
        // TODO: Use/Extract the interfaces
        new TypeMap(typeof(CharacterDungeonProgressionService), typeof(CharacterDungeonProgressionService), TypeMapOption.None),
        new TypeMap(typeof(ItemReequiperService), typeof(ItemReequiperService), TypeMapOption.None),
        new TypeMap(typeof(RuneQuantityProvider), typeof(RuneQuantityProvider), TypeMapOption.None),
        new TypeMap(typeof(GuildKnightsProvider), typeof(GuildKnightsProvider), TypeMapOption.None),
        new TypeMap(typeof(GemTypeUsageProvider), typeof(GemTypeUsageProvider), TypeMapOption.None),
        new TypeMap(typeof(RuneValueProvider), typeof(RuneValueProvider), TypeMapOption.None),
        new TypeMap(typeof(BaseStatsIncreasingService), typeof(BaseStatsIncreasingService), TypeMapOption.None),
        new TypeMap(typeof(ScrapbookService), typeof(ScrapbookService), TypeMapOption.None),
    ];
}