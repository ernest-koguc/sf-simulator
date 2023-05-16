namespace SFSimulator.Core
{
    public interface ICalendarRewardProvider
    {
        CalendarRewardType GetNextReward();
        void SetCalendar(int calendar, int day, bool skipCalendar);
    }
}