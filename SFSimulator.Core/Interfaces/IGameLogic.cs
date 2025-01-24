namespace SFSimulator.Core;

public interface IGameLogic
{
    QuestValue GetMinimumQuestValue(int characterLevel, ExperienceBonus experienceBonus, GoldBonus goldBonus);

    #region Experience
    long GetAcademyHourlyProduction(int characterLevel, int academyLevel, bool experienceEvent);
    long GetDailyMissionExperience(int characterLevel, bool experienceEvent, int hydraHeads);
    long GetExperienceForNextLevel(int characterLevel);
    long GetDailyExperienceFromWheel(int characterLevel, IEnumerable<EventType> events, SpinAmountType spinAmount);
    long GetExperienceRewardFromCalendar(int characterLevel, int rewardSize);
    long GetXPFromGuildFight(int characterLevel, IEnumerable<EventType> events);
    long GetExperienceForDungeonEnemy(int level, DungeonEnemy dungeonEnemy);
    long GetExperienceRewardFromArena(int characterLevel, bool experienceEvent);
    long GetWeeklyTasksExperience(int characterLevel, ExperienceBonus experienceBonus);
    #endregion

    #region Gold
    decimal GetGoldFromGuardDuty(int level, GoldBonus? goldBonus, bool goldEvent);
    decimal GetDailyMissionGold(int level);
    decimal GetGoblinTasksBaseGold(int characterLevel);
    decimal GetHourlyGoldPitProduction(int characterLevel, int goldPitLevel, bool goldEvent);
    decimal GetGoldRewardFromCalendar(int characterLevel, int rewardSize);
    decimal GetDailyGoldFromDiceGame(int characterLevel, IEnumerable<EventType> events);
    decimal GetDailyGoldFromWheel(int characterLevel, IEnumerable<EventType> events, SpinAmountType spinAmount);
    decimal GetDailyGoldFromGemMine(int characterLevel, int gemMineLevel, int workers = 15);
    decimal GetGoldRewardFromArena(int characterLevel, int count, bool arenaScroll);
    #endregion
}