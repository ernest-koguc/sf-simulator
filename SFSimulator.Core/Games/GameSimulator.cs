using System.ComponentModel;

namespace SFSimulator.Core;

public class GameSimulator : IGameSimulator
{
    private readonly IThirstSimulator _thirstSimulator;
    private readonly IExpeditionService _expeditionService;
    private readonly IGameLogic _gameLogic;
    private readonly ICalendarRewardProvider _calendarRewardProvider;
    private readonly IScheduler _scheduler;
    private readonly IQuestChooser _questChooser;
    private readonly IDungeonSimulator _dungeonSimulator;
    private readonly IWeeklyTasksRewardProvider _weeklyTasksRewardProvider;
    private readonly List<ItemType> CurrentItemTypesForWitch = new();
    private List<EventType> CurrentEvents = new();
    private IItemBackPack ItemBackPack { get; set; } = null!;
    private bool IsExperienceEvent => CurrentEvents is not null && CurrentEvents.Contains(EventType.Experience);
    private bool IsGoldEvent => CurrentEvents is not null && CurrentEvents.Contains(EventType.Gold);
    private bool IsWitchEvent => CurrentEvents is not null && CurrentEvents.Contains(EventType.Witch);
    private int CurrentDay { get; set; }

    public List<SimulatedGains> SimulatedDays { get; set; } = null!;
    public Character Character { get; set; } = null!;
    public SimulationOptions SimulationOptions { get; set; } = null!;

    private SimulatedGains CurrentDayResult { get; set; } = default!;

    public GameSimulator(IGameLogic characterHelper, IThirstSimulator thirstSimulator, ICalendarRewardProvider calendarRewardProvider, IWeeklyTasksRewardProvider weeklyTasksRewardProvider,
            IScheduler scheduler, IQuestChooser questChooser, IDungeonSimulator dungeonSimulator, IExpeditionService expeditionService)
    {
        _gameLogic = characterHelper;
        _thirstSimulator = thirstSimulator;
        _calendarRewardProvider = calendarRewardProvider;
        _scheduler = scheduler;
        _questChooser = questChooser;
        _dungeonSimulator = dungeonSimulator;
        _weeklyTasksRewardProvider = weeklyTasksRewardProvider;
        _expeditionService = expeditionService;
    }

    public SimulationResult Run(int until, Character character, SimulationOptions simulationOptions, SimulationType simulationType)
    {
        SetSimulationOptions(character, simulationOptions);
        Func<int> lookUpValue;

        switch (simulationType)
        {
            case SimulationType.UntilDays:
                lookUpValue = () => CurrentDay - 1;
                break;
            case SimulationType.UntilLevel:
                lookUpValue = () => Character.Level;
                break;
            case SimulationType.UntilBaseStats:
                lookUpValue = () => Character.BaseStat;
                break;
            default:
                throw new InvalidEnumArgumentException(nameof(simulationType));
        }

        for (CurrentDay = 1; lookUpValue() < until; CurrentDay++)
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
            Level = Character.Level,
            Experience = Character.Experience,
            BaseStat = Character.BaseStat,
            Days = SimulatedDays.Count,
            SimulatedDays = SimulatedDays,
            AverageGains = averageGains,
            TotalGains = totalGains,
        };
    }

    private void SetSimulationOptions(Character character, SimulationOptions simulationOptions)
    {
        Character = character;
        SimulationOptions = simulationOptions;
        ItemBackPack = new ItemBackPack(new ItemGoldValueComparer(), 5 + SimulationOptions.TreasuryLevel);

        _thirstSimulator.ThirstSimulationOptions.HasGoldScroll = SimulationOptions.GoldBonus.HasGoldScroll;
        _thirstSimulator.ThirstSimulationOptions.GoldRuneBonus = SimulationOptions.GoldBonus.RuneBonus;
        _thirstSimulator.ThirstSimulationOptions.Mount = SimulationOptions.Mount;
        _thirstSimulator.ThirstSimulationOptions.DrinkBeerOneByOne = SimulationOptions.DrinkBeerOneByOne;

        if (SimulationOptions.ExpeditionsInsteadOfQuests)
        {
            _expeditionService.Options = SimulationOptions.ExpeditionOptions ?? throw new ArgumentException("ExpeditionOptions must be set when ExpeditionsInsteadOfQuests is true", nameof(SimulationOptions.ExpeditionOptions));
        }
        else
        {
            _questChooser.QuestOptions = SimulationOptions.QuestOptions ?? throw new ArgumentException("QuestOptions must be set when ExpeditionsInsteadOfQuests is false", nameof(SimulationOptions.QuestOptions));
        }

        _calendarRewardProvider.ConfigureCalendar(SimulationOptions.Calendar, SimulationOptions.CalendarDay, SimulationOptions.SkipCalendar);

        _scheduler.SetCustomSchedule(SimulationOptions.Schedule);

        SimulatedDays = new List<SimulatedGains>();
    }
    private void RunDay()
    {
        var schedule = _scheduler.GetCurrentSchedule();
        CurrentEvents = schedule.Events;

        PerformScheduleActions(schedule);

        SellItemsToWitch();

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

        var dailyQuestXP = _gameLogic.GetDailyMissionExperience(Character.Level, IsExperienceEvent, SimulationOptions.HydraHeads);
        GiveXPToCharacter(dailyQuestXP, GainSource.DAILY_TASKS);

        var dailyQuestGold = _gameLogic.GetDailyMissionGold(Character.Level);
        GiveGoldToCharacter(dailyQuestGold, GainSource.DAILY_TASKS);

        var arenaXP = 10 * _gameLogic.GetExperienceRewardFromArena(Character.Level, IsExperienceEvent);
        GiveXPToCharacter(arenaXP, GainSource.ARENA);

        var arenaGold = _gameLogic.GetGoldRewardFromArena(Character.Level, SimulationOptions.FightsForGold, SimulationOptions.GoldBonus.HasArenaGoldScroll);
        GiveGoldToCharacter(arenaGold, GainSource.ARENA);

        var goldFromWatch = SimulationOptions.DailyGuard * _gameLogic.GetGoldFromGuardDuty(Character.Level, SimulationOptions.GoldBonus, IsGoldEvent);
        GiveGoldToCharacter(goldFromWatch, GainSource.GUARD);

        var goldFromDiceGame = _gameLogic.GetDailyGoldFromDiceGame(Character.Level, CurrentEvents);
        GiveGoldToCharacter(goldFromDiceGame, GainSource.DICE_GAME);

        var guildFightsXp = (long)(24 / 11.5 * _gameLogic.GetXPFromGuildFight(Character.Level, CurrentEvents));
        GiveXPToCharacter(guildFightsXp, GainSource.GUILD_FIGHT);

        var calendarReward = _calendarRewardProvider.GetNextReward();
        GiveCalendarRewardToPlayer(calendarReward);
    }

    private void CollectWeeklyTasksRewards()
    {

        if (!SimulationOptions.WeeklyTasksOptions.DoWeeklyTasks)
        {
            return;
        }

        var weeklyTasksXP = _weeklyTasksRewardProvider.GetWeeklyExperience(Character.Level, SimulationOptions.ExperienceBonus, CurrentDay);
        GiveXPToCharacter(weeklyTasksXP, GainSource.WEEKLY_TASKS);

        var weeklyTasksGold = _weeklyTasksRewardProvider.GetWeeklyGold(Character.Level, CurrentDay);
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
            _gameLogic.GetMinimumQuestValue(Character.Level, SimulationOptions.ExperienceBonus, SimulationOptions.GoldBonus),
            Character.Level,
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
                _gameLogic.GetMinimumQuestValue(Character.Level, SimulationOptions.ExperienceBonus, SimulationOptions.GoldBonus),
                Character.Level);
        }
    }

    private void DoExpeditions(int thirst)
    {
        // TODO: Maybe do expeditions in smaller segments to account for level ups, especially on lower levels it can make a difference in how much xp you get (e.g. level one character levels up every expedition pretty much)
        var gold = _expeditionService.GetDailyExpeditionGold(Character.Level, SimulationOptions.GoldBonus, IsGoldEvent, SimulationOptions.Mount, thirst);
        GiveGoldToCharacter(gold, GainSource.EXPEDITION);
        var xp = _expeditionService.GetDailyExpeditionExperience(Character.Level, SimulationOptions.ExperienceBonus, IsExperienceEvent, SimulationOptions.Mount, thirst);
        GiveXPToCharacter(xp, GainSource.EXPEDITION);

        var items = _expeditionService.GetDailyExpeditionItems(Character.Level, thirst);
        foreach (var item in items)
        {
            var goldFromItem = ItemBackPack.AddItemToBackPack(item, CurrentItemTypesForWitch);

            if (goldFromItem.HasValue)
                GiveGoldToCharacter(goldFromItem.Value, GainSource.ITEM);
        }
    }

    private void SpinAbawuwuWheel()
    {
        var goldFromWheel = _gameLogic.GetDailyGoldFromWheel(Character.Level, CurrentEvents, SimulationOptions.SpinAmount);
        GiveGoldToCharacter(goldFromWheel, GainSource.WHEEL);

        var xpFromWheel = _gameLogic.GetDailyExperienceFromWheel(Character.Level, CurrentEvents, SimulationOptions.SpinAmount);
        GiveXPToCharacter(xpFromWheel, GainSource.WHEEL);

        //TODO: PET AND NORMAL ITEMS FROM WHEEL LOGIC
    }

    private void CollectResourcesFromBuildings()
    {
        var goldPitProduction = 24 * _gameLogic.GetHourlyGoldPitProduction(Character.Level, SimulationOptions.GoldPitLevel, IsGoldEvent);
        GiveGoldToCharacter(goldPitProduction, GainSource.GOLD_PIT);

        var academyExperienceProduction = 24 * _gameLogic.GetAcademyHourlyProduction(Character.Level, SimulationOptions.AcademyLevel, IsExperienceEvent);
        GiveXPToCharacter(academyExperienceProduction, GainSource.ACADEMY);

        var goldFromGems = _gameLogic.GetDailyGoldFromGemMine(Character.Level, SimulationOptions.GemMineLevel);
        GiveGoldToCharacter(goldFromGems, GainSource.GEM);

        var questsFromTimeMachine = _thirstSimulator.GenerateQuestsFromTimeMachine(20, _gameLogic.GetMinimumQuestValue(Character.Level, SimulationOptions.ExperienceBonus, SimulationOptions.GoldBonus));

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

        Character.Gold += gold;
        if (Character.Gold > 10000000)
        {
            Character.BaseStat += (int)Math.Floor(Character.Gold / 10000000);
            Character.Gold %= 10000000;
        }

        var baseStatGain = CurrentDayResult.BaseStatGain;

        var baseStats = Math.Round(gold / 10000000, 3);
        baseStatGain[source] += baseStats;
        baseStatGain[GainSource.TOTAL] += baseStats;
    }
    private void GiveXPToCharacter(long xp, GainSource source)
    {
        if (xp < 0)
            throw new ArgumentOutOfRangeException(nameof(xp));

        if (Character.Level == 800)
            return;

        Character.Experience += xp;

        if (Character.Experience >= _gameLogic.GetExperienceForNextLevel(Character.Level))
        {
            LevelUpCharacter();
        }

        var xpGains = CurrentDayResult.ExperienceGain;

        xpGains[source] += xp;
        xpGains[GainSource.TOTAL] += xp;
    }
    private void LevelUpCharacter()
    {
        Character.Experience -= _gameLogic.GetExperienceForNextLevel(Character.Level);
        Character.Level++;

        if (SimulationOptions.SwitchPriority && Character.Level == SimulationOptions.SwitchLevel)
        {
            if (SimulationOptions.ExpeditionOptionsAfterSwitch is not null)
                _expeditionService.Options = SimulationOptions.ExpeditionOptionsAfterSwitch.Value;
            if (SimulationOptions.QuestOptionsAfterSwitch is not null)
                _questChooser.QuestOptions = SimulationOptions.QuestOptionsAfterSwitch.Value;
        }
    }
    private void GiveCalendarRewardToPlayer(CalendarRewardType calendarReward)
    {
        if (calendarReward == CalendarRewardType.ONE_BOOK)
        {
            var xp = _gameLogic.GetExperienceRewardFromCalendar(Character.Level, 1);
            GiveXPToCharacter(xp, GainSource.CALENDAR);
            return;
        }
        if (calendarReward == CalendarRewardType.TWO_BOOKS)
        {
            var xp = _gameLogic.GetExperienceRewardFromCalendar(Character.Level, 2);
            GiveXPToCharacter(xp, GainSource.CALENDAR);
            return;
        }
        if (calendarReward == CalendarRewardType.THREE_BOOKS)
        {
            var xp = _gameLogic.GetExperienceRewardFromCalendar(Character.Level, 3);
            GiveXPToCharacter(xp, GainSource.CALENDAR);
            return;
        }
        if (calendarReward == CalendarRewardType.LEVEL_UP)
        {
            var xp = _gameLogic.GetExperienceForNextLevel(Character.Level);
            GiveXPToCharacter(xp, GainSource.CALENDAR);
            return;
        }

        if (calendarReward == CalendarRewardType.ONE_GOLDBAR)
        {
            var gold = _gameLogic.GetGoldRewardFromCalendar(Character.Level, 1);
            GiveGoldToCharacter(gold, GainSource.CALENDAR);
            return;
        }
        if (calendarReward == CalendarRewardType.THREE_GOLDBARS)
        {
            var gold = _gameLogic.GetGoldRewardFromCalendar(Character.Level, 2);
            GiveGoldToCharacter(gold, GainSource.CALENDAR);
            return;
        }

        //TODO FRUITS LOGIC AND ATTRIBUTES - CLASS AS NECESSARY INPUT??
    }
}
