using System.ComponentModel;

namespace SFSimulator.Core;

public class GameSimulator(IGameLogic gameLogic, IThirstSimulator thirstSimulator, ICalendarRewardProvider calendarRewardProvider, IWeeklyTasksRewardProvider weeklyTasksRewardProvider,
        IScheduler scheduler, IQuestChooser questChooser, CharacterDungeonProgressionService characterDungeonProgressionService, IExpeditionService expeditionService) : IGameSimulator
{
    private readonly IThirstSimulator _thirstSimulator = thirstSimulator;
    private readonly IExpeditionService _expeditionService = expeditionService;
    private readonly IGameLogic _gameLogic = gameLogic;
    private readonly ICalendarRewardProvider _calendarRewardProvider = calendarRewardProvider;
    private readonly IScheduler _scheduler = scheduler;
    private readonly IQuestChooser _questChooser = questChooser;
    private readonly CharacterDungeonProgressionService _characterDungeonProgressionService = characterDungeonProgressionService;
    private readonly IWeeklyTasksRewardProvider _weeklyTasksRewardProvider = weeklyTasksRewardProvider;
    private readonly List<ItemType> CurrentItemTypesForWitch = [];
    private List<EventType> CurrentEvents { get; set; } = [];
    private ItemBackPack ItemBackPack { get; set; } = null!;
    private bool IsExperienceEvent => CurrentEvents is not null && CurrentEvents.Contains(EventType.Experience);
    private bool IsGoldEvent => CurrentEvents is not null && CurrentEvents.Contains(EventType.Gold);
    private bool IsWitchEvent => CurrentEvents is not null && CurrentEvents.Contains(EventType.Witch);
    private int CurrentDay { get; set; }

    public List<SimulatedGains> SimulatedDays { get; set; } = null!;
    public SimulationOptions SimulationOptions { get; set; } = null!;

    private SimulatedGains CurrentDayResult { get; set; } = default!;

    public SimulationResult Run(SimulationOptions simulationOptions, SimulationFinishCondition simulationFinishCondition)
    {
        SetSimulationOptions(simulationOptions);

        Func<int> lookUpValue = simulationFinishCondition.FinishCondition switch
        {
            SimulationFinishConditionType.UntilDays => () => CurrentDay - 1,
            SimulationFinishConditionType.UntilLevel => () => SimulationOptions.Level,
            SimulationFinishConditionType.UntilBaseStats => () => SimulationOptions.BaseStat,
            _ => throw new InvalidEnumArgumentException(nameof(simulationFinishCondition)),
        };

        for (CurrentDay = 1; lookUpValue() < simulationFinishCondition.Until; CurrentDay++)
        {
            var dayResult = new SimulatedGains { DayIndex = CurrentDay };
            SimulatedDays.Add(dayResult);
            CurrentDayResult = dayResult;
            RunDay();
        }

        return CreateResult();
    }

    private SimulationResult CreateResult()
    {
        var totalGains = new SimulatedGains();
        var averageGains = new SimulatedGains();

        foreach (var day in SimulatedDays)
        {
            // Remove empty gains
            day.BaseStatGain = day.BaseStatGain.Where(pair => pair.Value != 0).ToDictionary(pair => pair.Key, pair => pair.Value);
            day.ExperienceGain = day.ExperienceGain.Where(pair => pair.Value != 0).ToDictionary(pair => pair.Key, pair => pair.Value);

            foreach (var gain in day.BaseStatGain.Keys)
            {
                totalGains.BaseStatGain[gain] += day.BaseStatGain[gain];
            }
            foreach (var gain in day.ExperienceGain.Keys)
            {
                totalGains.ExperienceGain[gain] += day.ExperienceGain[gain];
            }
        }
        foreach (var key in totalGains.BaseStatGain.Keys)
        {
            averageGains.BaseStatGain[key] = totalGains.BaseStatGain[key] / SimulatedDays.Count;
        }
        foreach (var key in totalGains.ExperienceGain.Keys)
        {
            averageGains.ExperienceGain[key] = totalGains.ExperienceGain[key] / SimulatedDays.Count;
        }

        return new SimulationResult
        {
            Level = SimulationOptions.Level,
            Experience = SimulationOptions.Experience,
            BaseStat = SimulationOptions.BaseStat,
            Days = SimulatedDays.Count,
            SimulatedDays = SimulatedDays,
            AverageGains = averageGains,
            TotalGains = totalGains,
        };
    }

    private void SetSimulationOptions(SimulationOptions simulationOptions)
    {
        SimulationOptions = simulationOptions;
        ItemBackPack = new ItemBackPack(new ItemGoldValueComparer(), 5 + SimulationOptions.TreasuryLevel);

        _thirstSimulator.ThirstSimulationOptions.HasGoldScroll = SimulationOptions.GoldBonus.HasGoldScroll;
        _thirstSimulator.ThirstSimulationOptions.GoldRuneBonus = SimulationOptions.GoldBonus.RuneBonus / 100M;
        _thirstSimulator.ThirstSimulationOptions.Mount = SimulationOptions.Mount;
        _thirstSimulator.ThirstSimulationOptions.DrinkBeerOneByOne = SimulationOptions.DrinkBeerOneByOne;

        if (SimulationOptions.ExpeditionsInsteadOfQuests)
        {
            _expeditionService.Options = SimulationOptions.ExpeditionOptions ?? throw new ArgumentException("ExpeditionOptions must be set when ExpeditionsInsteadOfQuests is true", nameof(simulationOptions));
        }
        else
        {
            _questChooser.QuestOptions = SimulationOptions.QuestOptions ?? throw new ArgumentException("QuestOptions must be set when ExpeditionsInsteadOfQuests is false", nameof(simulationOptions));
        }

        _calendarRewardProvider.ConfigureCalendar(SimulationOptions.Calendar, SimulationOptions.CalendarDay, SimulationOptions.SkipCalendar);

        _scheduler.SetCustomSchedule(SimulationOptions.Schedule);

        // TODO: Add CharacterDungeonProgressionService options; Also adjust the service that uses the options
        if (SimulationOptions.DoDungeons)
            _characterDungeonProgressionService.InitCharacterDungeonState(SimulationOptions);

        SimulatedDays = [];
    }

    private void RunDay()
    {
        var schedule = _scheduler.GetCurrentSchedule();
        CurrentEvents = schedule.Events;

        PerformScheduleActions(schedule);

        SellItemsToWitch();

        DoDungeonProgression();

        if (SimulationOptions.ExpeditionsInsteadOfQuests)
        {
            DoExpeditions(SimulationOptions.DailyThirst);
        }
        else
        {
            DoThirst(SimulationOptions.DailyThirst);
        }

        CollectWeeklyTasksRewards();

        SpinAbawuwuWheel();

        CollectResourcesFromBuildings();

        var dailyQuestXP = _gameLogic.GetDailyMissionExperience(SimulationOptions.Level, IsExperienceEvent, SimulationOptions.HydraHeads);
        GiveXPToCharacter(dailyQuestXP, GainSource.DAILY_TASKS);

        var dailyQuestGold = _gameLogic.GetDailyMissionGold(SimulationOptions.Level);
        GiveGoldToCharacter(dailyQuestGold, GainSource.DAILY_TASKS);

        var arenaXP = 10 * _gameLogic.GetExperienceRewardFromArena(SimulationOptions.Level, IsExperienceEvent);
        GiveXPToCharacter(arenaXP, GainSource.ARENA);

        var arenaGold = _gameLogic.GetGoldRewardFromArena(SimulationOptions.Level, SimulationOptions.FightsForGold, SimulationOptions.GoldBonus.HasArenaGoldScroll);
        GiveGoldToCharacter(arenaGold, GainSource.ARENA);

        var goldFromWatch = SimulationOptions.DailyGuard * _gameLogic.GetGoldFromGuardDuty(SimulationOptions.Level, SimulationOptions.GoldBonus, IsGoldEvent);
        GiveGoldToCharacter(goldFromWatch, GainSource.GUARD);

        var goldFromDiceGame = _gameLogic.GetDailyGoldFromDiceGame(SimulationOptions.Level, CurrentEvents);
        GiveGoldToCharacter(goldFromDiceGame, GainSource.DICE_GAME);

        var guildFightsXp = (long)(24 / 11.5 * _gameLogic.GetXPFromGuildFight(SimulationOptions.Level, CurrentEvents));
        GiveXPToCharacter(guildFightsXp, GainSource.GUILD_FIGHT);

        var calendarReward = _calendarRewardProvider.GetNextReward();
        GiveCalendarRewardToPlayer(calendarReward);
    }

    private void DoDungeonProgression()
    {
        if (SimulationOptions.DoDungeons)
        {
            _characterDungeonProgressionService.ProgressThrough(SimulationOptions, result =>
            {
                GiveXPToCharacter(result.Experience, GainSource.DUNGEON);
                GiveGoldToCharacter(result.Gold, GainSource.DUNGEON);
            }, CurrentDay);
        }
    }

    private void CollectWeeklyTasksRewards()
    {

        if (!SimulationOptions.WeeklyTasksOptions.DoWeeklyTasks)
        {
            return;
        }

        var weeklyTasksXP = _weeklyTasksRewardProvider.GetWeeklyExperience(SimulationOptions.Level, SimulationOptions.ExperienceBonus, CurrentDay);
        GiveXPToCharacter(weeklyTasksXP, GainSource.WEEKLY_TASKS);

        var weeklyTasksGold = _weeklyTasksRewardProvider.GetWeeklyGold(SimulationOptions.Level, CurrentDay);
        GiveGoldToCharacter(weeklyTasksGold, GainSource.WEEKLY_TASKS);

        if (SimulationOptions.WeeklyTasksOptions.DrinkExtraBeer)
        {
            var extraThirst = _weeklyTasksRewardProvider.GetWeeklyThirst(CurrentDay);
            if (SimulationOptions.ExpeditionsInsteadOfQuests)
            {
                DoExpeditions(extraThirst);
            }
            else
            {
                DoThirst(extraThirst);
            }
        }
    }

    private void PerformScheduleActions(ScheduleDay schedule)
    {
        foreach (var action in schedule.Actions)
        {
            SimulationOptions.PerformAction(_thirstSimulator.ThirstSimulationOptions, action);
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
            GiveGoldToCharacter(gold.Value, GainSource.ITEM);
    }

    private void DoThirst(int thirst)
    {
        var quests = _thirstSimulator.StartThirst(thirst,
            _gameLogic.GetMinimumQuestValue(SimulationOptions.Level, SimulationOptions.ExperienceBonus, SimulationOptions.GoldBonus),
            SimulationOptions.Level,
            CurrentEvents
            );

        var questList = new List<Quest>();

        while (quests is not null)
        {
            var choosenQuest = _questChooser.ChooseBestQuest(quests);
            questList.Add(choosenQuest);

            GiveGoldToCharacter(choosenQuest.Gold, GainSource.QUEST);
            GiveXPToCharacter((long)choosenQuest.Experience, GainSource.QUEST);

            var goldFromItem = ItemBackPack.AddItemToBackPack(choosenQuest.Item, CurrentItemTypesForWitch);

            if (goldFromItem.HasValue)
                GiveGoldToCharacter(goldFromItem.Value, GainSource.ITEM);


            quests = _thirstSimulator.NextQuests(choosenQuest,
                _gameLogic.GetMinimumQuestValue(SimulationOptions.Level, SimulationOptions.ExperienceBonus, SimulationOptions.GoldBonus),
                SimulationOptions.Level);
        }
    }

    private void DoExpeditions(int thirst)
    {
        // TODO: Maybe do expeditions in smaller segments to account for level ups, especially on lower levels it can make a difference in how much xp you get (e.g. level one character levels up every expedition pretty much)
        var gold = _expeditionService.GetDailyExpeditionGold(SimulationOptions.Level, SimulationOptions.GoldBonus, IsGoldEvent, SimulationOptions.Mount, thirst);
        GiveGoldToCharacter(gold, GainSource.EXPEDITION);
        var xp = _expeditionService.GetDailyExpeditionExperience(SimulationOptions.Level, SimulationOptions.ExperienceBonus, IsExperienceEvent, SimulationOptions.Mount, thirst);
        GiveXPToCharacter(xp, GainSource.EXPEDITION);

        var items = _expeditionService.GetDailyExpeditionItems(SimulationOptions.Level, thirst);
        foreach (var item in items)
        {
            var goldFromItem = ItemBackPack.AddItemToBackPack(item, CurrentItemTypesForWitch);

            if (goldFromItem.HasValue)
                GiveGoldToCharacter(goldFromItem.Value, GainSource.ITEM);
        }
    }

    private void SpinAbawuwuWheel()
    {
        var goldFromWheel = _gameLogic.GetDailyGoldFromWheel(SimulationOptions.Level, CurrentEvents, SimulationOptions.SpinAmount);
        GiveGoldToCharacter(goldFromWheel, GainSource.WHEEL);

        var xpFromWheel = _gameLogic.GetDailyExperienceFromWheel(SimulationOptions.Level, CurrentEvents, SimulationOptions.SpinAmount);
        GiveXPToCharacter(xpFromWheel, GainSource.WHEEL);

        //TODO: PET AND NORMAL ITEMS FROM WHEEL LOGIC
    }

    private void CollectResourcesFromBuildings()
    {
        var goldPitProduction = 24 * _gameLogic.GetHourlyGoldPitProduction(SimulationOptions.Level, SimulationOptions.GoldPitLevel, IsGoldEvent);
        GiveGoldToCharacter(goldPitProduction, GainSource.GOLD_PIT);

        var academyExperienceProduction = 24 * _gameLogic.GetAcademyHourlyProduction(SimulationOptions.Level, SimulationOptions.AcademyLevel, IsExperienceEvent);
        GiveXPToCharacter(academyExperienceProduction, GainSource.ACADEMY);

        var goldFromGems = _gameLogic.GetDailyGoldFromGemMine(SimulationOptions.Level, SimulationOptions.GemMineLevel);
        GiveGoldToCharacter(goldFromGems, GainSource.GEM);

        var questsFromTimeMachine = _thirstSimulator.GenerateQuestsFromTimeMachine(20, _gameLogic.GetMinimumQuestValue(SimulationOptions.Level, SimulationOptions.ExperienceBonus, SimulationOptions.GoldBonus));

        foreach (var quest in questsFromTimeMachine)
        {
            GiveGoldToCharacter(quest.Gold, GainSource.TIME_MACHINE);
            GiveXPToCharacter((long)quest.Experience, GainSource.TIME_MACHINE);
        }
    }

    private void GiveGoldToCharacter(decimal gold, GainSource source)
    {
        if (gold < 0)
            throw new ArgumentOutOfRangeException(nameof(gold));

        SimulationOptions.Gold += gold;
        if (SimulationOptions.Gold > 10000000)
        {
            SimulationOptions.BaseStat += (int)Math.Floor(SimulationOptions.Gold / 10000000);
            SimulationOptions.Gold %= 10000000;
        }

        var baseStatGain = CurrentDayResult.BaseStatGain;

        var baseStats = Math.Round(gold / 10000000, 3);
        baseStatGain[source] += baseStats;
        baseStatGain[GainSource.TOTAL] += baseStats;
    }
    private void GiveXPToCharacter(long xp, GainSource source)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(xp);

        if (SimulationOptions.Level == 800)
            return;

        SimulationOptions.Experience += xp;

        if (SimulationOptions.Experience >= _gameLogic.GetExperienceForNextLevel(SimulationOptions.Level))
        {
            LevelUpCharacter();
        }

        var xpGains = CurrentDayResult.ExperienceGain;

        xpGains[source] += xp;
        xpGains[GainSource.TOTAL] += xp;
    }
    private void LevelUpCharacter()
    {
        SimulationOptions.Experience -= _gameLogic.GetExperienceForNextLevel(SimulationOptions.Level);
        SimulationOptions.Level++;

        if (SimulationOptions.SwitchPriority && SimulationOptions.Level == SimulationOptions.SwitchLevel)
        {
            if (SimulationOptions.ExpeditionOptionsAfterSwitch is not null)
                _expeditionService.Options = SimulationOptions.ExpeditionOptionsAfterSwitch;
            if (SimulationOptions.QuestOptionsAfterSwitch is not null)
                _questChooser.QuestOptions = SimulationOptions.QuestOptionsAfterSwitch;
        }
    }
    private void GiveCalendarRewardToPlayer(CalendarRewardType calendarReward)
    {
        if (calendarReward == CalendarRewardType.ONE_BOOK)
        {
            var xp = _gameLogic.GetExperienceRewardFromCalendar(SimulationOptions.Level, 1);
            GiveXPToCharacter(xp, GainSource.CALENDAR);
            return;
        }
        if (calendarReward == CalendarRewardType.TWO_BOOKS)
        {
            var xp = _gameLogic.GetExperienceRewardFromCalendar(SimulationOptions.Level, 2);
            GiveXPToCharacter(xp, GainSource.CALENDAR);
            return;
        }
        if (calendarReward == CalendarRewardType.THREE_BOOKS)
        {
            var xp = _gameLogic.GetExperienceRewardFromCalendar(SimulationOptions.Level, 3);
            GiveXPToCharacter(xp, GainSource.CALENDAR);
            return;
        }
        if (calendarReward == CalendarRewardType.LEVEL_UP)
        {
            var xp = _gameLogic.GetExperienceForNextLevel(SimulationOptions.Level);
            GiveXPToCharacter(xp, GainSource.CALENDAR);
            return;
        }

        if (calendarReward == CalendarRewardType.ONE_GOLDBAR)
        {
            var gold = _gameLogic.GetGoldRewardFromCalendar(SimulationOptions.Level, 1);
            GiveGoldToCharacter(gold, GainSource.CALENDAR);
            return;
        }
        if (calendarReward == CalendarRewardType.THREE_GOLDBARS)
        {
            var gold = _gameLogic.GetGoldRewardFromCalendar(SimulationOptions.Level, 2);
            GiveGoldToCharacter(gold, GainSource.CALENDAR);
            return;
        }

        //TODO FRUITS LOGIC AND ATTRIBUTES - CLASS AS NECESSARY INPUT??
    }
}