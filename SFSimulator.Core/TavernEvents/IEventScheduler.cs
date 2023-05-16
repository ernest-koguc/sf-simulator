namespace SFSimulator.Core
{
    public interface IEventScheduler
    {
        List<EventType> GetCurrentEvents(int currentDay);
    }
}