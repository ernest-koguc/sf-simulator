namespace SFSimulator.Core;

public interface IGameFormulasService
{
    QuestValue GetMinimumQuestValue(int characterLevel, ExperienceBonus experienceBonus, GoldBonus goldBonus);
    double GetDailyPetFoodFromWheel(int characterLevel, List<EventType> events, SpinAmountType spinAmount);
    bool DoesDungeonEnemyDropItem(DungeonEnemy dungeonEnemy);

    #region Experience
    long GetAcademyHourlyProduction(int characterLevel, int academyLevel, bool experienceEvent);
    long GetDailyMissionExperience(int characterLevel, bool experienceEvent, int hydraHeads);
    long GetExperienceForNextLevel(int characterLevel);
    long GetDailyExperienceFromWheel(int characterLevel, IEnumerable<EventType> events, SpinAmountType spinAmount);
    long GetExperienceRewardFromCalendar(int characterLevel, int rewardSize);
    long GetXPFromGuildFight(int characterLevel, IEnumerable<EventType> events);
    long GetExperienceForDungeonEnemy(DungeonEnemy dungeonEnemy);
    long GetExperienceForPetDungeonEnemy(int characterLevel);
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
    decimal GetExpeditionChestGold(int characterLevel, GoldBonus goldBonus, bool isGoldEvent, MountType mount, int thirst);
    decimal GetExpeditionMidwayGold(int characterLevel, GoldBonus goldBonus, bool isGoldEvent, MountType mount, int thirst);
    decimal GetExpeditionFinalGold(int characterLevel, GoldBonus goldBonus, bool isGoldEvent, MountType mount, int thirst);
    decimal GetGoldForDungeonEnemy(DungeonEnemy dungeonEnemy);
    #endregion
}