using AutoMapper;

namespace SFSimulator.Core
{
    public class GameSimulator : IGameSimulator
    {
        private readonly IThirstSimulator _thirstSimulator;
        private readonly ICharacterHelper _characterHelper;
        private readonly ICalendarRewardProvider _calendarRewardProvider;
        private readonly IEventScheduler _eventScheduler;
        private readonly IQuestChooser _questChooser;
        private readonly IMapper _mapper;
        private List<EventType> CurrentEvents = new();
        private IItemBackPack ItemBackPack { get; set; } = null!;
        private bool IsExperienceEvent => CurrentEvents is not null && CurrentEvents.Contains(EventType.EXPERIENCE);
        private bool IsGoldEvent => CurrentEvents is not null && CurrentEvents.Contains(EventType.GOLD);
        private bool IsWitchEvent => CurrentEvents is not null && CurrentEvents.Contains(EventType.WITCH);

        private List<ItemType> CurrentItemTypesForWitch = new();

        private int CurrentDay { get; set; }

        public SimulationResult SimulationResult { get; set; } = new SimulationResult();
        public Character Character { get; set; } = null!;
        public SimulationOptions SimulationOptions { get; set; } = null!;

        public GameSimulator(ICharacterHelper characterHelper, IThirstSimulator thirstSimulator, ICalendarRewardProvider calendarRewardProvider, IEventScheduler eventScheduler, IQuestChooser questChooser, IMapper mapper)
        {
            _characterHelper = characterHelper;
            _thirstSimulator = thirstSimulator;
            _calendarRewardProvider = calendarRewardProvider;
            _eventScheduler = eventScheduler;
            _questChooser = questChooser;
            _mapper = mapper;
        }
        public SimulationResult RunDays(int days, Character character, SimulationOptions simulationOptions)
        {
            SetSimulationOptions(character, simulationOptions, days);

            for (CurrentDay = 1; CurrentDay <= days; CurrentDay++)
            {
                SimulationResult.SimulatedDays.Add(new() { DayIndex = CurrentDay });
                RunDay();
            }

            SimulationResult.CharacterAfter = _mapper.Map<CharacterDTO>(Character);
            CalculateStatistics();

            return SimulationResult;
        }
        public SimulationResult RunLevels(int level, Character character, SimulationOptions simulationOptions)
        {
            SetSimulationOptions(character, simulationOptions);

            for (CurrentDay = 1; Character.Level < level; CurrentDay++)
            {
                SimulationResult.Days++;
                SimulationResult.SimulatedDays.Add(new() { DayIndex = CurrentDay });
                RunDay();
            }

            SimulationResult.CharacterAfter = _mapper.Map<CharacterDTO>(Character);
            CalculateStatistics();

            return SimulationResult;
        }

        private void CalculateStatistics()
        {
            var totalGains = new SimulatedGains();
            var averageGains = new SimulatedGains();
            var experienceGains = SimulationResult.SimulatedDays.Select(o => o.ExperienceGain);

            foreach(var day in SimulationResult.SimulatedDays)
            {
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
                averageGains.BaseStatGain[key] = totalGains.BaseStatGain[key] / SimulationResult.Days;
            }
            foreach (var key in totalGains.ExperienceGain.Keys)
            {
                averageGains.ExperienceGain[key] = totalGains.ExperienceGain[key] / SimulationResult.Days;
            }

            SimulationResult.TotalGains = totalGains;
            SimulationResult.AverageGains = averageGains;
        }

        private void SetSimulationOptions(Character character, SimulationOptions simulationOptions, int? days = null)
        {
            Character = character;
            SimulationOptions = simulationOptions;
            ItemBackPack = new ItemBackPack(new ItemGoldValueComparer(), 5 + Character.TreasuryLevel);
            _thirstSimulator.ThirstSimulationOptions.HasGoldScroll = SimulationOptions.GoldBonus.HasGoldScroll;
            _thirstSimulator.ThirstSimulationOptions.GoldRuneBonus = SimulationOptions.GoldBonus.RuneBonus;
            _calendarRewardProvider.SetCalendar(1, 1, simulationOptions.SkipCalendar);

            var charPreviously = _mapper.Map<CharacterDTO>(Character);

            SimulationResult = new SimulationResult { CharacterName=simulationOptions.CharacterName, Days = days ?? 0, SimulatedDays = new(), CharacterPreviously = charPreviously };
        }
        private void RunDay()
        {
            CurrentEvents = _eventScheduler.GetCurrentEvents(CurrentDay);

            var calendarReward = _calendarRewardProvider.GetNextReward();
            GiveCalendarRewardToPlayer(calendarReward);


            if (Character.GoldPitLevel < 100 && CurrentDay % 14 == 0)
                Character.GoldPitLevel++;

            SellItemsToWitch();

            DoThirst();

            SpinAbawuwuWheel();

            CollectResourcesFromBuildings();

            var dailyQuestXP = _characterHelper.GetDailyMissionReward(Character.Level, IsExperienceEvent, Character.HydraHeads);

            GiveXPToCharacter(dailyQuestXP, GainSource.DAILY_MISSION);

            var arenaXP = 10 * _characterHelper.GetExperienceRewardFromArena(Character.Level, IsExperienceEvent);

            GiveXPToCharacter(arenaXP, GainSource.ARENA);

            var goldFromWatch = 23 * _characterHelper.GetGoldFromGuardDuty(Character.Level, SimulationOptions.GoldBonus, IsGoldEvent);
            GiveGoldToCharacter(goldFromWatch, GainSource.GUARD);

            var goldFromDiceGame = _characterHelper.GetDailyGoldFromDiceGame(Character.Level, CurrentEvents!);
            GiveGoldToCharacter(goldFromDiceGame, GainSource.DICE_GAME);

            var guildFightsXp = (int)(24 / 11.5 * _characterHelper.GetXPFromGuildFight(Character.Level, CurrentEvents));
            GiveXPToCharacter(guildFightsXp, GainSource.GUILD_FIGHT);
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

        private void DoThirst()
        {
            var quests = _thirstSimulator.StartThirst(SimulationOptions.DailyThirst,
                _characterHelper.GetMinimumQuestValue(Character.Level, SimulationOptions.ExperienceBonus, SimulationOptions.GoldBonus),
                Character.Level,
                Character.Mount,
                CurrentEvents,
                SimulationOptions.DrinkBeerOneByOne
                );
            var questList = new List<Quest>();

            while (quests is not null)
            {
                var choosenQuest = _questChooser.ChooseBestQuest(quests, SimulationOptions.QuestPriority, SimulationOptions.QuestChooserAI, SimulationOptions.HybridRatio);
                questList.Add(choosenQuest);

                GiveGoldToCharacter((float)choosenQuest.Gold, GainSource.QUEST);
                GiveXPToCharacter((int)choosenQuest.Experience, GainSource.QUEST);

                var goldFromItem = ItemBackPack.AddItemToBackPack(choosenQuest.Item, CurrentItemTypesForWitch);

                if (goldFromItem.HasValue)
                    GiveGoldToCharacter(goldFromItem.Value, GainSource.ITEM);


                quests = _thirstSimulator.NextQuests(choosenQuest,
                    _characterHelper.GetMinimumQuestValue(Character.Level, SimulationOptions.ExperienceBonus, SimulationOptions.GoldBonus),
                    Character.Level,
                    Character.Mount);
            }
            var totalXP = questList.Sum(q => q.Experience);
        }

        private void SpinAbawuwuWheel()
        {
            var goldFromWheel = _characterHelper.GetDailyGoldFromWheel(Character.Level, CurrentEvents);
            GiveGoldToCharacter(goldFromWheel, GainSource.WHEEL);

            var xpFromWheel = _characterHelper.GetDailyExperienceFromWheel(Character.Level, CurrentEvents);
            GiveXPToCharacter(xpFromWheel, GainSource.WHEEL);

            //TODO: PET AND NORMAL ITEMS FROM WHEEL LOGIC
        }

        private void CollectResourcesFromBuildings()
        {
            var goldPitProduction = 24 * _characterHelper.GetHourlyGoldPitProduction(Character.Level, Character.GoldPitLevel, IsGoldEvent);
            GiveGoldToCharacter(goldPitProduction, GainSource.GOLD_PIT);

            var academyExperienceProduction = 24 * _characterHelper.GetAcademyHourlyProduction(Character.Level, Character.AcademyLevel, IsExperienceEvent);
            GiveXPToCharacter(academyExperienceProduction, GainSource.ACADEMY);

            var goldFromGems = _characterHelper.GetDailyGoldFromGemMine(Character.Level, Character.GemMineLevel);
            GiveGoldToCharacter(goldFromGems, GainSource.GEM);

            var questsFromTimeMachine = _thirstSimulator.GenerateQuestsFromTimeMachine(20, _characterHelper.GetMinimumQuestValue(Character.Level, SimulationOptions.ExperienceBonus, SimulationOptions.GoldBonus), Character.Mount);

            foreach (var quest in questsFromTimeMachine)
            {
                GiveGoldToCharacter((float)quest.Gold, GainSource.TIME_MACHINE);
                GiveXPToCharacter((int)quest.Experience, GainSource.TIME_MACHINE);
            }
        }

        private void GiveGoldToCharacter(float gold, GainSource source)
        {
            if (gold < 0)
                throw new ArgumentOutOfRangeException(nameof(gold));

            Character.Gold += gold;
            if (Character.Gold > 10000000)
            {
                Character.BaseStat += (int)Math.Floor(Character.Gold / 10000000);
                Character.Gold %= 10000000;
            }

            var baseStatGain = SimulationResult.SimulatedDays.FirstOrDefault(d => d.DayIndex == CurrentDay)?.BaseStatGain;

            _ = baseStatGain ?? throw new NullReferenceException();

            var baseStats = (float)Math.Round(gold / 10000000, 3);
            baseStatGain[source] += baseStats;
            baseStatGain[GainSource.TOTAL] += baseStats;
        }
        private void GiveXPToCharacter(int xp, GainSource source)
        {
            if (xp < 0)
                throw new ArgumentOutOfRangeException(nameof(xp));

            if (Character.Level == 800)
                return;

            Character.Experience += xp;

            if (Character.Experience >= _characterHelper.GetExperienceForNextLevel(Character.Level))
            {
                LevelUpCharacter();
            }

            var xpGains = SimulationResult.SimulatedDays.FirstOrDefault(d => d.DayIndex == CurrentDay)?.ExperienceGain;

            _ = xpGains ?? throw new NullReferenceException();
            xpGains[source] += xp;
            xpGains[GainSource.TOTAL] += xp;
        }
        private void LevelUpCharacter()
        {
            Character.Experience -= _characterHelper.GetExperienceForNextLevel(Character.Level);
            Character.Level++;

            if (SimulationOptions.SwitchPriority && Character.Level == SimulationOptions.SwitchLevel)
                SimulationOptions.QuestPriority = SimulationOptions.PriorityAfterSwitch;

            if (SimulationOptions.SavedDungeonsStart is not null && Character.Level == SimulationOptions.SavedDungeonsStart)
                Character.Level += SimulationOptions.SavedLevels;
        }
        private void GiveCalendarRewardToPlayer(CalendarRewardType calendarReward)
        {
            if (calendarReward == CalendarRewardType.ONE_BOOK)
            {
                var xp = _characterHelper.GetExperienceRewardFromCalendar(Character.Level, 1);
                GiveXPToCharacter(xp, GainSource.CALENDAR);
                return;
            }
            if (calendarReward == CalendarRewardType.TWO_BOOKS)
            {
                var xp = _characterHelper.GetExperienceRewardFromCalendar(Character.Level, 2);
                GiveXPToCharacter(xp, GainSource.CALENDAR);
                return;
            }
            if (calendarReward == CalendarRewardType.THREE_BOOKS)
            {
                var xp = _characterHelper.GetExperienceRewardFromCalendar(Character.Level, 3);
                GiveXPToCharacter(xp, GainSource.CALENDAR);
                return;
            }
            if (calendarReward == CalendarRewardType.LEVEL_UP)
            {
                var xp = _characterHelper.GetExperienceForNextLevel(Character.Level);
                GiveXPToCharacter(xp, GainSource.CALENDAR);
                return;
            }

            if (calendarReward == CalendarRewardType.ONE_GOLDBAR)
            {
                var gold = _characterHelper.GetGoldRewardFromCalendar(Character.Level, 1);
                GiveGoldToCharacter(gold, GainSource.CALENDAR);
                return;
            }
            if (calendarReward == CalendarRewardType.THREE_GOLDBARS)
            {
                var gold = _characterHelper.GetGoldRewardFromCalendar(Character.Level, 2);
                GiveGoldToCharacter(gold, GainSource.CALENDAR);
                return;
            }

            //TODO FRUITS LOGIC AND ATTRIBUTES  - CLASS AS NECESSARY INPUT??
        }
    }
}
