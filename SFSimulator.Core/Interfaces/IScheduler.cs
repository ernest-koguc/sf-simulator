namespace SFSimulator.Core;

public interface IScheduler
{
    void SetStartingPoint(int week, int day);
    void SetCustomSchedule(Dictionary<(int Week, int Day), List<EventType>> schedule);
    void SetSchedule(EventScheduleType scheduleType);
    List<EventType> GetEvents();

}