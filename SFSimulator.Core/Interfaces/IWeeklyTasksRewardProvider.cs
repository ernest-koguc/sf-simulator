namespace SFSimulator.Core;

public interface IWeeklyTasksRewardProvider
{
    long GetWeeklyExperience(int characterLevel, ExperienceBonus experienceBonus, int day);
    decimal GetWeeklyGold(int characterLevel, int day);
    int GetWeeklyThirst(int day);
}
