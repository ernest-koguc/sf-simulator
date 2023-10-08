namespace SFSimulator.Core
{
    public interface IScheduler
    {
        void SetStartingPoint(int week, int day);
        ScheduleDay GetCurrentSchedule();
        void SetCustomSchedule(Dictionary<(int Week, int Day), ScheduleDay> schedule);
        void SetDefaultSchedule();
    }
}