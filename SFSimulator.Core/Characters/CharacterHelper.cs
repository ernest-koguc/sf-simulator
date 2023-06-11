namespace SFSimulator.Core
{
    public class CharacterHelper : ICharacterHelper
    {
        private readonly IValuesReader _valuesReader;
        private Dictionary<int, int> ExperienceForNextLevel { get; set; }

        private readonly ICurvesHelper _curvesHelper;
        public CharacterHelper(IValuesReader valuesReader, ICurvesHelper curvesHelper)
        {
            _valuesReader = valuesReader ?? throw new ArgumentNullException(nameof(valuesReader));
            _curvesHelper = curvesHelper ?? throw new ArgumentNullException(nameof(curvesHelper));

            ExperienceForNextLevel = _valuesReader.ReadExperienceForNextLevel();
        }
        public int GetExperienceForNextLevel(int characterLevel)
        {
            if (characterLevel > 393) return 1500000000;
            return ExperienceForNextLevel[characterLevel];
        }
        public int GetDailyMissionReward(int characterLevel, bool xpEvent, int hydraHeads = 0)
        {
            var xp = GetExperienceForNextLevel(characterLevel);
            var multiplier = 1.5 + 0.75 * (characterLevel - 1);
            var basic = xp / multiplier;
            var daily = Convert.ToInt32(Math.Floor((1 + 0.25 * hydraHeads) * basic));
            if (xpEvent)
                daily *= 2;

            return daily;
        }
        public QuestValue GetMinimumQuestValue(int characterLevel, ExperienceBonus experienceBonus, GoldBonus goldBonus)
        {
            var xp = GetExperienceForNextLevel(characterLevel) / (1.5 + 0.75 * (characterLevel - 1));
            var xpMin = (int)(experienceBonus.CombinedBonus() * xp / 11 / Math.Max(1, Math.Exp(30090.33 / 5000000 * (characterLevel - 99))));
            var gold = _curvesHelper.GoldCurve[characterLevel] * 12 / 1000;
            var goldMin = goldBonus.CombinedBonus() * (gold / 11);
            return new QuestValue(goldMin, xpMin);
        }
        public float GetGoldFromGuardDuty(int characterLevel, GoldBonus? goldBonus, bool goldEvent)
        {
            var hourlyGuard = _curvesHelper.GoldCurve[characterLevel] * 12 / 1000 / 3;

            if (goldBonus != null)
                hourlyGuard = hourlyGuard * (1 + goldBonus.GuildBonus + goldBonus.Tower);

            if (goldEvent)
                hourlyGuard *= 5;

            if (hourlyGuard > 10000000)
                return 10000000;

            return (float)hourlyGuard;
        }
        public float GetHourlyGoldPitProduction(int characterLevel, int goldPitLevel, bool goldEvent)
        {
            var goldCurveValue = _curvesHelper.GoldCurve[characterLevel] * 12 / 1000;
            var goldPitProduction = goldCurveValue * goldPitLevel / 75f * (1 + Math.Max(0, goldPitLevel - 15) / 100f);

            if (goldEvent)
                goldPitProduction *= 1.5f;

            return (float)goldPitProduction;
        }
        public int GetAcademyHourlyProduction(int characterLevel, int academyLevel, bool xpEvent)
        {
            var xp = GetExperienceForNextLevel(characterLevel);
            var multiplier = 1.5 + 0.75 * (characterLevel - 1);
            var basic = xp / multiplier;
            var hbase = basic / 30 / Math.Max(1, Math.Exp(30090.33 / 5000000 * (characterLevel - 99)));
            var hourly = (int)(academyLevel * hbase);

            if (xpEvent)
                hourly *= 2;

            return hourly;
        }

        public int GetExperienceRewardFromArena(int characterLevel, bool xpEvent)
        {
            var xp = GetExperienceForNextLevel(characterLevel);
            var multiplier = 1.5 + 0.75 * (characterLevel - 1);
            var basic = xp / multiplier;
            var arena = (int)basic / 10;

            if (xpEvent)
                arena *= 2;

            return arena;
        }

        public int GetExperienceRewardFromCalendar(int characterLevel, int rewardSize)
        {
            if (rewardSize < 1 || rewardSize > 3)
                throw new ArgumentOutOfRangeException(nameof(rewardSize));

            var xp = GetExperienceForNextLevel(characterLevel);
            var multiplier = 5d * (4 - rewardSize);
            var calendar = Math.Ceiling(xp / multiplier);

            return (int)calendar;
        }

        public float GetGoldRewardFromCalendar(int characterLevel, int rewardSize)
        {
            if (rewardSize < 1 || rewardSize > 2)
                throw new ArgumentOutOfRangeException(nameof(rewardSize));

            var goldCurveMultiplier = _curvesHelper.GoldCurve[characterLevel] * 12 / 1000 / 3;
            var reward = goldCurveMultiplier * 25;

            if (rewardSize == 1)
                reward = 3 * reward / 10;

            return (float)reward;
        }

        public float GetDailyGoldFromDiceGame(int characterLevel, IEnumerable<EventType> events)
        {
            var averageGold = 0.3555f;

            var dailySpins = 10;

            if (events.Contains(EventType.LuckyDay))
                dailySpins = 20;


            var goldFromGuard = GetGoldFromGuardDuty(characterLevel, null, false);
            var goldReward = goldFromGuard * dailySpins * averageGold;

            if (events.Contains(EventType.Gold))
                goldReward *= 5;

            return goldReward;
        }

        public float GetDailyGoldFromWheel(int characterLevel, IEnumerable<EventType> events, SpinAmountType spinAmount)
        {
            var isLuckyDay = events.Contains(EventType.LuckyDay);

            var spins = 1;

            if (isLuckyDay && spinAmount == SpinAmountType.Max)
                spins = 40;
            else if (spinAmount == SpinAmountType.Max)
                spins = 20;

            var goldRewardChance = 0.1f;

            var goldFromGuard = GetGoldFromGuardDuty(characterLevel, null, false);
            var goldReward = goldFromGuard * 20;

            if (goldReward > 15000000)
                goldReward = 15000000;
            if (events.Contains(EventType.Gold))
                goldReward *= 5;

            var gold = goldReward * spins * goldRewardChance;

            return gold;
        }

        public int GetDailyExperienceFromWheel(int characterLevel, IEnumerable<EventType> events, SpinAmountType spinAmount)
        {
            var isLuckyDay = events.Contains(EventType.LuckyDay);

            var spins = 1;

            if (isLuckyDay && spinAmount == SpinAmountType.Max)
                spins = 40;
            else if (spinAmount == SpinAmountType.Max)
                spins = 20;

            var xp = GetExperienceForNextLevel(characterLevel);
            var multiplier = 1.5 + 0.75 * (characterLevel - 1);
            var basic = xp / multiplier;

            var xpReward = Math.Truncate(basic / Math.Max(1, Math.Exp(30090.33 / 5000000 * (characterLevel - 99))));

            var smallXpRewardChance = 0.1f;
            var bigXpRewardChance = 0.1f;

            var totalXp = spins * smallXpRewardChance * xpReward / 2 + spins * bigXpRewardChance * xpReward;

            if (events.Contains(EventType.Experience))
                totalXp *= 2;

            return (int)totalXp;
        }
        public int GetXPFromGuildFight(int characterLevel, IEnumerable<EventType> events)
        {
            var xpBase = GetExperienceForNextLevel(characterLevel);
            var xp = Math.Floor(15 / 16d * xpBase / (characterLevel + 5d));

            if (events.Contains(EventType.Experience))
                xp *= 2;

            return (int)xp;
        }

        public float GetDailyGoldFromGemMine(int characterLevel, int gemMineLevel, int workers = 15)
        {
            if (gemMineLevel == 0)
                return 0;

            float time = (1 - workers * 0.05f) * GetMiningDuration(gemMineLevel) * 60;
            float gold = 24 * 60 / time * GetAverageGemGold(characterLevel, gemMineLevel);

            return gold;
        }
        private float GetAverageGemGold(int characterLevel, int gemMineLevel)
        {
            var goldCurve = _curvesHelper.GoldCurve[characterLevel] * 12 / 1000;


            var gem = goldCurve / 6;
            gem = Math.Min(10000000, gem * GetGemMineMultiplier(gemMineLevel));

            var gem2 = _curvesHelper.GoldCurve[characterLevel + 5] / 10;
            gem2 = Math.Min(10E6, gem2 * 2 * GetGemMineMultiplier(gemMineLevel) / 100);

            var gem3 = _curvesHelper.GoldCurve[characterLevel + 10] / 10;
            gem3 = Math.Min(10E6, gem3 * 2 * GetGemMineMultiplier(gemMineLevel) / 100);

            var avgValue = (gem + gem2 + gem3) / 3;

            return (float)avgValue;
        }
        private int GetGemMineMultiplier(int gemMineLevel)
        {
            if (gemMineLevel <= 1)
            {
                return 1;
            }
            else if (gemMineLevel >= 14)
            {
                return 24;
            }
            else
            {
                switch (gemMineLevel)
                {
                    case 2: return 2;
                    case 3: return 3;
                    case 4: return 4;
                    case 5: return 6;
                    case 6: return 8;
                    case 7: return 10;
                    case 8: return 12;
                    case 9: return 14;
                    case 10: return 16;
                    case 11: return 18;
                    case 12: return 20;
                    case 13: return 22;
                }
            }
            return 11;
        }
        private int GetMiningDuration(int gemMineLevel)
        {
            return gemMineLevel switch
            {
                1 => 1,
                2 => 2,
                3 => 3,
                4 => 4,
                5 => 6,
                6 => 8,
                7 => 10,
                8 => 12,
                9 => 14,
                10 => 16,
                11 => 18,
                12 => 20,
                13 => 24,
                14 => 28,
                15 => 32,
                16 => 30,
                17 => 28,
                18 => 26,
                19 => 25,
                _ => 25,
            };
        }
    }
}
