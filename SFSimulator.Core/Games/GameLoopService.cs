using System.ComponentModel;
using System.Diagnostics;

namespace SFSimulator.Core;

public class GameLoopService(IGameFormulasService gameFormulasService, IThirstSimulator thirstSimulator, ICalendarRewardProvider calendarRewardProvider,
    IWeeklyTasksRewardProvider weeklyTasksRewardProvider, IScheduler scheduler, ICharacterDungeonProgressionService characterDungeonProgressionService,
    IExpeditionService expeditionService, IBaseStatsIncreasingService baseStatsIncreasingService, IScrapbookService scrapbookService,
    IPotionService potionService, IPortalService portalService, IGuildRaidService guildRaidService, IPetProgressionService petProgressionService,
    IAuraProgressService auraProgressService, IRuneQuantityProvider runeQuantityProvider) : IGameLoopService
{
    private readonly IThirstSimulator _thirstSimulator = thirstSimulator;
    private readonly IExpeditionService _expeditionService = expeditionService;
    private readonly IBaseStatsIncreasingService _baseStatsIncreasingService = baseStatsIncreasingService;
    private readonly IGameFormulasService _gameFormulasService = gameFormulasService;
    private readonly ICalendarRewardProvider _calendarRewardProvider = calendarRewardProvider;
    private readonly IScheduler _scheduler = scheduler;
    private readonly ICharacterDungeonProgressionService _characterDungeonProgressionService = characterDungeonProgressionService;
    private readonly IWeeklyTasksRewardProvider _weeklyTasksRewardProvider = weeklyTasksRewardProvider;
    private readonly IScrapbookService _scrapbookService = scrapbookService;
    private readonly IPotionService _potionService = potionService;
    private readonly IPortalService _portalService = portalService;
    private readonly IGuildRaidService _guildRaidService = guildRaidService;
    private readonly IPetProgressionService _petProgressionService = petProgressionService;
    private readonly IAuraProgressService _auraProgressService = auraProgressService;
    private readonly IRuneQuantityProvider _runeQuantityProvider = runeQuantityProvider;

    private readonly List<ItemType> CurrentItemTypesForWitch = [];
    private List<EventType> CurrentEvents { get; set; } = [];
    private ItemBackPack ItemBackPack { get; set; } = null!;
    private bool IsExperienceEvent => CurrentEvents.Contains(EventType.Experience);
    private bool IsGoldEvent => CurrentEvents.Contains(EventType.Gold);
    private bool IsWitchEvent => CurrentEvents.Contains(EventType.Witch);
    private int CurrentDay { get; set; }
    private ContextSnapshot BeforeSimulation { get; set; } = default!;
    private Dictionary<AchievementType, SimulationAchievement> Achievements { get; set; } = new();
    private List<SimulatedGains> SimulatedDays { get; set; } = null!;
    private SimulationContext SimulationContext { get; set; } = null!;
    private SimulatedGains CurrentDayGains { get; set; } = default!;

    public Task<SimulationResult?> Run(SimulationContext simulationContext, Action<SimulationProgress> progressCallback)
    {
        try
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            SetupContext(simulationContext);

            Func<int> lookUpValue = simulationContext.FinishCondition.FinishWhen switch
            {
                SimulationFinishConditionType.UntilDays => () => CurrentDay - 1,
                SimulationFinishConditionType.UntilLevel => () =>
                    SimulationContext.Level > simulationContext.FinishCondition.Until ? simulationContext.FinishCondition.Until : SimulationContext.Level,
                SimulationFinishConditionType.UntilBaseStats => () =>
                    SimulationContext.BaseStat > simulationContext.FinishCondition.Until ? simulationContext.FinishCondition.Until : SimulationContext.BaseStat,
                _ => throw new InvalidEnumArgumentException(nameof(simulationContext.FinishCondition.FinishWhen)),
            };
            var startingValue = lookUpValue();
            var simulateUntil = simulationContext.FinishCondition.Until - lookUpValue();

            var progressStopwatch = new Stopwatch();
            progressStopwatch.Start();
            for (CurrentDay = 1; lookUpValue() < simulationContext.FinishCondition.Until; CurrentDay++)
            {
                var dayResult = new SimulatedGains { DayIndex = CurrentDay };
                SimulatedDays.Add(dayResult);
                CurrentDayGains = dayResult;
                RunDay();
                if (progressStopwatch.ElapsedMilliseconds > 100)
                {
                    progressCallback(new(lookUpValue() - startingValue, CurrentDay, simulateUntil));
                    progressStopwatch.Restart();
                }
            }
            progressCallback(new(lookUpValue() - startingValue, CurrentDay - 1, simulateUntil));
            progressStopwatch.Stop();
            Achievements.Add(AchievementType.SimulationFinish, new(CurrentDay - 1, AchievementType.SimulationFinish));


            stopwatch.Stop();
            Console.WriteLine(SimulationContext.ToString());
            Console.WriteLine($"Simulation ended, time elapsed: {stopwatch.ElapsedMilliseconds} ms");
            return Task.FromResult<SimulationResult?>(CreateResult());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Task.FromResult<SimulationResult?>(null);
        }
    }

    private SimulationResult CreateResult()
    {
        foreach (var day in SimulatedDays)
        {
            // Remove empty gains
            day.BaseStatGain = day.BaseStatGain
                .Where(pair => pair.Value != 0)
                .OrderBy(pair => pair.Key)
                .ToDictionary(pair => pair.Key, pair => pair.Value);
            day.ExperienceGain = day.ExperienceGain
                .Where(pair => pair.Value != 0)
                .OrderBy(pair => pair.Key)
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        return new SimulationResult
        {
            Days = SimulatedDays.Count,
            SimulatedDays = SimulatedDays,
            BeforeSimulation = BeforeSimulation,
            AfterSimulation = new(SimulationContext.BaseStat, SimulationContext.Level, SimulationContext.Experience),
            Achievements = Achievements.Values.ToList()
        };
    }

    private void SetupContext(SimulationContext simulationContext)
    {
        foreach (var companion in simulationContext.Companions)
        {
            companion.Character = simulationContext;
        }

        SimulationContext = simulationContext;
        ItemBackPack = new ItemBackPack(new ItemGoldValueComparer(), 5 + SimulationContext.TreasuryLevel);
        Achievements = new() { { AchievementType.SimulationStart, new(1, AchievementType.SimulationStart) } };
        BeforeSimulation = new ContextSnapshot(simulationContext.BaseStat, simulationContext.Level, simulationContext.Experience);

        _thirstSimulator.ThirstSimulationOptions.Mount = SimulationContext.Mount;
        _characterDungeonProgressionService.ResetProgress();

        _expeditionService.Options = SimulationContext.ExpeditionOptions ?? throw new ArgumentException("ExpeditionOptions must be set when ExpeditionsInsteadOfQuests is true", nameof(simulationContext));

        _calendarRewardProvider.ConfigureCalendar(SimulationContext.Calendar, SimulationContext.CalendarDay, SimulationContext.SkipCalendar);

        _scheduler.SetCustomSchedule(SimulationContext.Schedule);

        SimulationContext.Potions = _potionService.GetPotions(SimulationContext.Class);


        if (SimulationContext.DoDungeons)
        {
            _characterDungeonProgressionService.InitCharacterDungeonState(SimulationContext);
            _characterDungeonProgressionService.ReequipOptions = SimulationContext.ReequipOptions;
            _characterDungeonProgressionService.DungeonOptions = SimulationContext.DungeonOptions;
        }

        var dungeons = _characterDungeonProgressionService.GetDungeons(SimulationContext);

        _scrapbookService.InitScrapbook(dungeons, simulationContext);

        _portalService.SetUpPortalState(simulationContext);

        _guildRaidService.SetUpGuildRaidsState(simulationContext);

        _runeQuantityProvider.Setup(simulationContext);

        SimulatedDays = [];
    }

    private void RunDay()
    {
        var schedule = _scheduler.GetCurrentSchedule();
        CurrentEvents = schedule.Events;

        PerformScheduleActions(schedule);

        SellItemsToWitch();

        _runeQuantityProvider.IncreaseRuneQuantity(SimulationContext, CurrentDay);

        _auraProgressService.IncreaseAuraProgress(SimulationContext, CurrentEvents.Contains(EventType.Toilet));

        DoDungeonProgression();

        DoExpeditions(SimulationContext.DailyThirst);

        CollectWeeklyTasksRewards();

        SpinAbawuwuWheel();

        CollectResourcesFromBuildings();

        var dailyQuestXP = _gameFormulasService.GetDailyMissionExperience(SimulationContext.Level, IsExperienceEvent, SimulationContext.HydraHeads);
        GiveXPToCharacter(dailyQuestXP, GainSource.DailyTasks);

        var dailyQuestGold = _gameFormulasService.GetDailyMissionGold(SimulationContext.Level);
        GiveGoldToCharacter(dailyQuestGold, GainSource.DailyTasks);

        var arenaXP = 10 * _gameFormulasService.GetExperienceRewardFromArena(SimulationContext.Level, IsExperienceEvent);
        GiveXPToCharacter(arenaXP, GainSource.Arena);

        var goldFromWatch = SimulationContext.DailyGuard * _gameFormulasService.GetGoldFromGuardDuty(SimulationContext.Level, SimulationContext.GoldBonus, IsGoldEvent);
        GiveGoldToCharacter(goldFromWatch, GainSource.Guard);

        var goldFromDiceGame = _gameFormulasService.GetDailyGoldFromDiceGame(SimulationContext.Level, CurrentEvents);
        GiveGoldToCharacter(goldFromDiceGame, GainSource.DiceGame);

        var guildFightsXp = (long)(24 / 11.5 * _gameFormulasService.GetXPFromGuildFight(SimulationContext.Level, CurrentEvents));
        GiveXPToCharacter(guildFightsXp, GainSource.GuildFight);

        var calendarReward = _calendarRewardProvider.GetNextReward();
        GiveCalendarRewardToPlayer(calendarReward);

        _portalService.Progress(CurrentDay, SimulationContext);
        _guildRaidService.Progress(CurrentDay, SimulationContext,
            (newPictures) => _scrapbookService.UpdateScrapbook(SimulationContext, newPictures));

        DoPetsProgression();
    }

    private void DoPetsProgression()
    {
        _petProgressionService.DoPetArenaFights(CurrentDay, SimulationContext.Pets, CurrentEvents.Contains(EventType.Pets));

        if (SimulationContext.DoPetsDungeons)
        {
            _petProgressionService.ProgressThroughDungeons(CurrentDay, SimulationContext, CurrentEvents, result =>
            {
                GiveXPToCharacter(result.Experience, GainSource.Pets);
            });
        }

        if (SimulationContext.SellPetFood)
        {
            var soldFoodGold = _petProgressionService.SellPetFood(SimulationContext.Pets, SimulationContext.Level);
            GiveGoldToCharacter(soldFoodGold, GainSource.Pets);
        }
    }

    private void DoDungeonProgression()
    {
        if (!SimulationContext.DoDungeons)
        {
            return;
        }

        _characterDungeonProgressionService.ProgressThrough(SimulationContext, result =>
        {
            _scrapbookService.UpdateScrapbook(SimulationContext, result);
            GiveXPToCharacter(result.Experience, GainSource.Dungeon);
            GiveGoldToCharacter(result.Gold, GainSource.Dungeon);

            if (result.Item is not null)
            {
                var goldFromItem = ItemBackPack.AddItemToBackPack(result.Item, CurrentItemTypesForWitch);
                if (goldFromItem.HasValue)
                    GiveGoldToCharacter(goldFromItem.Value, GainSource.Item);
            }
            if (result.DungeonEnemy.Dungeon.Type == DungeonTypeEnum.Tower)
            {
                SimulationContext.GoldBonus.Tower = result.DungeonEnemy.Position;
            }
        }, CurrentDay);
    }

    private void CollectWeeklyTasksRewards()
    {
        if (!SimulationContext.WeeklyTasksOptions.DoWeeklyTasks)
        {
            return;
        }

        var weeklyTasksXP = _weeklyTasksRewardProvider.GetWeeklyExperience(SimulationContext.Level, SimulationContext.ExperienceBonus, CurrentDay);
        GiveXPToCharacter(weeklyTasksXP, GainSource.WeeklyTasks);

        var weeklyTasksGold = _weeklyTasksRewardProvider.GetWeeklyGold(SimulationContext.Level, CurrentDay);
        GiveGoldToCharacter(weeklyTasksGold, GainSource.WeeklyTasks);

        if (SimulationContext.WeeklyTasksOptions.DrinkExtraBeer)
        {
            var extraThirst = _weeklyTasksRewardProvider.GetWeeklyThirst(CurrentDay);
            DoExpeditions(extraThirst);
        }
    }

    private void PerformScheduleActions(ScheduleDay schedule)
    {
        foreach (var action in schedule.Actions)
        {
            SimulationContext.PerformAction(_thirstSimulator.ThirstSimulationOptions, action);
        }
    }

    private void SellItemsToWitch()
    {
        CurrentItemTypesForWitch.Clear();

        if (IsWitchEvent)
        {
            CurrentItemTypesForWitch.AddRange(Enumerable.Range(1, 9).Select(i => (ItemType)i));
        }
        else
        {
            CurrentItemTypesForWitch.Add((ItemType)Random.Shared.Next(1, 10));
        }

        var gold = ItemBackPack.SellSpecifiedItemTypeToWitch(CurrentItemTypesForWitch);

        if (gold.HasValue)
            GiveGoldToCharacter(gold.Value, GainSource.Item);
    }

    private void DoExpeditions(int thirst)
    {
        // TODO: Maybe do expeditions in smaller segments to account for level ups, especially on lower
        // levels it can make a difference in how much xp you get (e.g. level one character levels up every expedition pretty much)
        // Also this might make more sense if we consider the switch period when picking gold in midway reward is better than pet food
        // be aware that some of the calculation need to know the total amount of thirst, so maybe this should be encapsulated in the expedition service
        // and some sort of action to update the simulation context should be part of the API?
        var gold = _expeditionService.GetDailyExpeditionGold(SimulationContext.Level, SimulationContext.GoldBonus, IsGoldEvent, SimulationContext.Mount, thirst);
        GiveGoldToCharacter(gold, GainSource.Expedition);
        var xp = _expeditionService.GetDailyExpeditionExperience(SimulationContext.Level, SimulationContext.ExperienceBonus, IsExperienceEvent, SimulationContext.Mount, thirst);
        GiveXPToCharacter(xp, GainSource.Expedition);

        var items = _expeditionService.GetDailyExpeditionItems(SimulationContext.Level, thirst);
        foreach (var item in items)
        {
            var goldFromItem = ItemBackPack.AddItemToBackPack(item, CurrentItemTypesForWitch);

            if (goldFromItem.HasValue)
                GiveGoldToCharacter(goldFromItem.Value, GainSource.Item);
        }

        var petFood = _expeditionService.GetDailyExpeditionPetFood(SimulationContext.Level, SimulationContext.GoldBonus, CurrentEvents, SimulationContext.Mount, thirst);
        _petProgressionService.GivePetFood(SimulationContext.Pets, petFood);
    }

    private void SpinAbawuwuWheel()
    {
        var goldFromWheel = _gameFormulasService.GetDailyGoldFromWheel(SimulationContext.Level, CurrentEvents, SimulationContext.SpinAmount);
        GiveGoldToCharacter(goldFromWheel, GainSource.Wheel);

        var xpFromWheel = _gameFormulasService.GetDailyExperienceFromWheel(SimulationContext.Level, CurrentEvents, SimulationContext.SpinAmount);
        GiveXPToCharacter(xpFromWheel, GainSource.Wheel);

        var petFood = _gameFormulasService.GetDailyPetFoodFromWheel(SimulationContext.Level, CurrentEvents, SimulationContext.SpinAmount);
        _petProgressionService.GivePetFood(SimulationContext.Pets, petFood);

        //TODO: NORMAL ITEMS FROM WHEEL LOGIC
    }

    private void CollectResourcesFromBuildings()
    {
        var goldPitProduction = 24 * _gameFormulasService.GetHourlyGoldPitProduction(SimulationContext.Level, SimulationContext.GoldPitLevel, IsGoldEvent);
        GiveGoldToCharacter(goldPitProduction, GainSource.GoldPit);

        var academyExperienceProduction = 24 * _gameFormulasService.GetAcademyHourlyProduction(SimulationContext.Level, SimulationContext.AcademyLevel, IsExperienceEvent);
        GiveXPToCharacter(academyExperienceProduction, GainSource.Academy);

        var goldFromGems = _gameFormulasService.GetDailyGoldFromGemMine(SimulationContext.Level, SimulationContext.GemMineLevel);
        GiveGoldToCharacter(goldFromGems, GainSource.Gem);

        var questsFromTimeMachine = _thirstSimulator.GenerateQuestsFromTimeMachine(20, _gameFormulasService.GetMinimumQuestValue(SimulationContext.Level, SimulationContext.ExperienceBonus, SimulationContext.GoldBonus));

        foreach (var quest in questsFromTimeMachine)
        {
            GiveGoldToCharacter(quest.Gold, GainSource.TimeMachine);
            GiveXPToCharacter((long)quest.Experience, GainSource.TimeMachine);
        }
    }

    private void GiveGoldToCharacter(decimal gold, GainSource source)
    {
        if (gold < 0)
            throw new ArgumentOutOfRangeException(nameof(gold));

        SimulationContext.Gold += gold;

        var baseStatGain = CurrentDayGains.BaseStatGain;

        _baseStatsIncreasingService.IncreaseBaseStats(SimulationContext);

        var baseStats = Math.Round(gold / 10000000, 3);
        baseStatGain[source] += baseStats;
        baseStatGain[GainSource.Total] += baseStats;

        AchievementType? achievementType = SimulationContext.BaseStat switch
        {
            >= 1000000 when BeforeSimulation.BaseStat < 1000000 => AchievementType.BaseStat1000000,
            >= 900000 when BeforeSimulation.BaseStat < 900000 => AchievementType.BaseStat900000,
            >= 800000 when BeforeSimulation.BaseStat < 800000 => AchievementType.BaseStat800000,
            >= 700000 when BeforeSimulation.BaseStat < 700000 => AchievementType.BaseStat700000,
            >= 600000 when BeforeSimulation.BaseStat < 600000 => AchievementType.BaseStat600000,
            >= 500000 when BeforeSimulation.BaseStat < 500000 => AchievementType.BaseStat500000,
            >= 400000 when BeforeSimulation.BaseStat < 400000 => AchievementType.BaseStat400000,
            >= 350000 when BeforeSimulation.BaseStat < 350000 => AchievementType.BaseStat350000,
            >= 300000 when BeforeSimulation.BaseStat < 300000 => AchievementType.BaseStat300000,
            >= 250000 when BeforeSimulation.BaseStat < 250000 => AchievementType.BaseStat250000,
            >= 200000 when BeforeSimulation.BaseStat < 200000 => AchievementType.BaseStat200000,
            >= 150000 when BeforeSimulation.BaseStat < 150000 => AchievementType.BaseStat150000,
            >= 100000 when BeforeSimulation.BaseStat < 100000 => AchievementType.BaseStat100000,
            >= 75000 when BeforeSimulation.BaseStat < 75000 => AchievementType.BaseStat75000,
            >= 50000 when BeforeSimulation.BaseStat < 50000 => AchievementType.BaseStat50000,
            >= 10000 when BeforeSimulation.BaseStat < 10000 => AchievementType.BaseStat10000,
            >= 1000 when BeforeSimulation.BaseStat < 1000 => AchievementType.BaseStat1000,
            _ => null
        };

        if (achievementType is not null)
        {
            Achievements.TryAdd(achievementType.Value, new SimulationAchievement { Type = achievementType.Value, Day = CurrentDay });
        }
    }
    private void GiveXPToCharacter(long xp, GainSource source)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(xp);

        if (SimulationContext.Level == 800)
            return;

        SimulationContext.Experience += xp;

        if (SimulationContext.Experience >= _gameFormulasService.GetExperienceForNextLevel(SimulationContext.Level))
        {
            LevelUpCharacter();
        }

        var xpGains = CurrentDayGains.ExperienceGain;

        xpGains[source] += xp;
        xpGains[GainSource.Total] += xp;
    }
    private void LevelUpCharacter()
    {
        SimulationContext.Experience -= _gameFormulasService.GetExperienceForNextLevel(SimulationContext.Level);
        SimulationContext.Level++;

        if (SimulationContext.SwitchPriority && SimulationContext.Level == SimulationContext.SwitchLevel)
        {
            if (SimulationContext.ExpeditionOptionsAfterSwitch is not null)
                _expeditionService.Options = SimulationContext.ExpeditionOptionsAfterSwitch;
        }

        _scrapbookService.UpdateScrapbook(SimulationContext);

        AchievementType? achievementType = SimulationContext.Level switch
        {
            100 when BeforeSimulation.Level < 100 => AchievementType.Level100,
            200 when BeforeSimulation.Level < 200 => AchievementType.Level200,
            300 when BeforeSimulation.Level < 300 => AchievementType.Level300,
            400 when BeforeSimulation.Level < 400 => AchievementType.Level400,
            500 when BeforeSimulation.Level < 500 => AchievementType.Level500,
            600 when BeforeSimulation.Level < 600 => AchievementType.Level600,
            700 when BeforeSimulation.Level < 700 => AchievementType.Level700,
            800 when BeforeSimulation.Level < 800 => AchievementType.Level800,
            _ => null
        };
        if (achievementType is not null)
        {
            Achievements.TryAdd(achievementType.Value, new SimulationAchievement { Type = achievementType.Value, Day = CurrentDay });
        }
    }

    private void GiveCalendarRewardToPlayer(CalendarRewardType calendarReward)
    {
        if (calendarReward == CalendarRewardType.ONE_BOOK)
        {
            var xp = _gameFormulasService.GetExperienceRewardFromCalendar(SimulationContext.Level, 1);
            GiveXPToCharacter(xp, GainSource.Calendar);
            return;
        }
        if (calendarReward == CalendarRewardType.TWO_BOOKS)
        {
            var xp = _gameFormulasService.GetExperienceRewardFromCalendar(SimulationContext.Level, 2);
            GiveXPToCharacter(xp, GainSource.Calendar);
            return;
        }
        if (calendarReward == CalendarRewardType.THREE_BOOKS)
        {
            var xp = _gameFormulasService.GetExperienceRewardFromCalendar(SimulationContext.Level, 3);
            GiveXPToCharacter(xp, GainSource.Calendar);
            return;
        }
        if (calendarReward == CalendarRewardType.LEVEL_UP)
        {
            var xp = _gameFormulasService.GetExperienceForNextLevel(SimulationContext.Level);
            GiveXPToCharacter(xp, GainSource.Calendar);
            return;
        }

        if (calendarReward == CalendarRewardType.ONE_GOLDBAR)
        {
            var gold = _gameFormulasService.GetGoldRewardFromCalendar(SimulationContext.Level, 1);
            GiveGoldToCharacter(gold, GainSource.Calendar);
            return;
        }
        if (calendarReward == CalendarRewardType.THREE_GOLDBARS)
        {
            var gold = _gameFormulasService.GetGoldRewardFromCalendar(SimulationContext.Level, 2);
            GiveGoldToCharacter(gold, GainSource.Calendar);
            return;
        }

        if (calendarReward == CalendarRewardType.SHADOW_FRUIT)
        {
            _petProgressionService.GivePetFood(SimulationContext.Pets, 5, PetElementType.Shadow);
        }

        if (calendarReward == CalendarRewardType.LIGHT_FRUIT)
        {
            _petProgressionService.GivePetFood(SimulationContext.Pets, 5, PetElementType.Light);
        }

        if (calendarReward == CalendarRewardType.EARTH_FRUIT)
        {
            _petProgressionService.GivePetFood(SimulationContext.Pets, 5, PetElementType.Earth);
        }

        if (calendarReward == CalendarRewardType.FIRE_FRUIT)
        {
            _petProgressionService.GivePetFood(SimulationContext.Pets, 5, PetElementType.Fire);
        }

        if (calendarReward == CalendarRewardType.WATER_FRUIT)
        {
            _petProgressionService.GivePetFood(SimulationContext.Pets, 5, PetElementType.Water);
        }

        //TODO ATTRIBUTES LOGIC - CLASS AS NECESSARY INPUT??
    }
}