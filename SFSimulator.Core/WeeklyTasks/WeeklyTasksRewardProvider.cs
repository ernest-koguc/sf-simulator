namespace SFSimulator.Core;

public class WeeklyTasksRewardProvider(IGameFormulasService _gameLogic) : IWeeklyTasksRewardProvider
{
    public decimal GetWeeklyGold(int characterLevel, int day)
    {
        if (day % 7 != 1) return 0M;

        var baseGold = _gameLogic.GetGoblinTasksBaseGold(characterLevel);
        var reward = baseGold * 40;

        var week = day / 7 + 1;

        // When it's weekly tasks with first chest as gold reward
        if (week % 10 == 7)
            reward += baseGold * 32;

        return reward;
    }

    public long GetWeeklyExperience(int characterLevel, ExperienceBonus experienceBonus, int day)
    {
        if (day % 7 != 1) return 0;

        // If not week 2 or 8 return no reward
        var week = day / 7 + 1;
        if (week % 10 != 2 && week % 10 != 8) return 0;

        return _gameLogic.GetWeeklyTasksExperience(characterLevel, experienceBonus);
    }

    public int GetWeeklyThirst(int day) => day % 7 == 1 ? 20 : 0;
}