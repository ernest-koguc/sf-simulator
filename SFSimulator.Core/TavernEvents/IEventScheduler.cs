
using QuestSimulator.Enums;

namespace QuestSimulator.TavernEvents
{
    public interface IEventScheduler
    {
        List<EventType> GetCurrentEvents(int currentDay);
    }
}