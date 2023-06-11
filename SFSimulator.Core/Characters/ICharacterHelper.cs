namespace SFSimulator.Core
{
    public interface ICharacterHelper
    {
        int GetAcademyHourlyProduction(int characterLevel, int academyLevel, bool experienceEvent);
        int GetDailyMissionReward(int characterLevel, bool experienceEvent, int hydraHeads);
        int GetExperienceForNextLevel(int characterLevel);
        QuestValue GetMinimumQuestValue(int characterLevel, ExperienceBonus experienceBonus, GoldBonus goldBonus);
        float GetGoldFromGuardDuty(int level, GoldBonus? goldBonus, bool goldEvent);
        float GetHourlyGoldPitProduction(int characterLevel, int goldPitLevel, bool goldEvent);
        int GetExperienceRewardFromCalendar(int characterLevel, int rewardSize);
        float GetGoldRewardFromCalendar(int characterLevel, int rewardSize);
        int GetExperienceRewardFromArena(int characterLevel, bool experienceEvent);
        float GetDailyGoldFromDiceGame(int characterLevel, IEnumerable<EventType> events);
        int GetDailyExperienceFromWheel(int characterLevel, IEnumerable<EventType> events, SpinAmountType spinAmount);
        float GetDailyGoldFromWheel(int characterLevel, IEnumerable<EventType> events, SpinAmountType spinAmount);
        int GetXPFromGuildFight(int characterLevel, IEnumerable<EventType> events);
        float GetDailyGoldFromGemMine(int characterLevel, int gemMineLevel, int workers = 15);
    }
}