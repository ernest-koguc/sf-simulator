namespace SFSimulator.Core
{
    public interface ICalendarRewardProvider
    {
        CalendarRewardType GetNextReward();
        void ConfigureCalendar(int calendar, int day, bool skipCalendar);
    }
}