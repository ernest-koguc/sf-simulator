namespace SFSimulator.Core;

public record class TypeMap(Type Implementation, Type Interface);

public static class TypesMapper
{
    public static IEnumerable<TypeMap> Types =>
    [
        new (typeof(GameFormulasService), typeof(IGameFormulasService)),
        new (typeof(ItemValueProvider), typeof(IItemValueProvider)),
        new (typeof(GameLoopService), typeof(IGameLoopService)),
        new (typeof(CalendarRewardProvider), typeof(ICalendarRewardProvider)),
        new (typeof(Scheduler), typeof(IScheduler)),
        new (typeof(QuestHelper), typeof(IQuestHelper)),
        new (typeof(QuestFactory), typeof(IQuestFactory)),
        new (typeof(ThirstSimulator), typeof(IThirstSimulator)),
        new (typeof(Curves), typeof(ICurves)),
        new (typeof(Random), typeof(Random)),
        new (typeof(ItemGenerator), typeof(IItemGenerator)),
        new (typeof(DungeonSimulator), typeof(IDungeonSimulator)),
        new (typeof(DungeonProvider), typeof(IDungeonProvider)),
        new (typeof(FightableContextFactory), typeof(IFightableContextFactory)),
        new (typeof(CritChanceProvider), typeof(ICritChanceProvider)),
        new (typeof(DamageProvider), typeof(IDamageProvider)),
        new (typeof(WeeklyTasksRewardProvider), typeof(IWeeklyTasksRewardProvider)),
        new (typeof(ExpeditionService), typeof(IExpeditionService)),
        new (typeof(BlackSmithAdvisor), typeof(IBlackSmithAdvisor)) ,
        new (typeof(GuildKnightsProvider), typeof(IGuildKnightsProvider)),
        new (typeof(BaseStatsIncreasingService), typeof(IBaseStatsIncreasingService)),
        new (typeof(RuneQuantityProvider), typeof(IRuneQuantityProvider)),
        new (typeof(RuneValueProvider), typeof(IRuneValueProvider)),
        new (typeof(GemTypeUsageProvider), typeof(IGemTypeUsageProvider)),
        new (typeof(CharacterDungeonProgressionService), typeof(ICharacterDungeonProgressionService)),
        new (typeof(ItemReequiperService), typeof(IItemReequiperService)),
        new (typeof(ScrapbookService), typeof(IScrapbookService)),
        new (typeof(PotionService), typeof(IPotionService)),
        new (typeof(PortalService), typeof(IPortalService)),
        new (typeof(GuildRaidService), typeof(IGuildRaidService)),
        new (typeof(PetFightableFactory), typeof(IPetFightableFactory)),
        new (typeof(PetPathProvider), typeof(IPetPathProvider)),
        new (typeof(PetProgressionService), typeof(IPetProgressionService)),
        new (typeof(PetUnlockerService), typeof(IPetUnlockerService)),
        new (typeof(AuraProgressService), typeof(IAuraProgressService)),
        new (typeof(WitchService), typeof(IWitchService)),
        new (typeof(FortressService), typeof(IFortressService)),
        new (typeof(UnderworldService), typeof(IUnderworldService)),
    ];
}