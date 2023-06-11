using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFSimulator.Core;

namespace SFSimulator.Tests
{
    [TestClass]
    public class EventSchedulerTests
    {


        [TestMethod]
        public void GetCurrentEvents_gives_proper_events()
        {

            var eventScheduler = new Scheduler();
            eventScheduler.SetDefaultSchedule();
            eventScheduler.SetStartingPoint(1, 1);
            for (var i = 1; i <= 84; i++)
            {
                var events = eventScheduler.GetCurrentSchedule().Events;
                if (i % 7 == 0 || i % 7 > 4)
                    Assert.IsTrue(events.Count > 0);
                else
                    Assert.IsTrue(events.Count == 0);
                if (i == 5)
                    Assert.IsTrue(events.Contains(EventType.Experience) && events.Count == 3);
            }


            eventScheduler = new Scheduler();
            eventScheduler.SetStartingPoint(7, 6);
            for (var i = 1; i <= 84; i++)
            {
                var events = eventScheduler.GetCurrentSchedule().Events;
                if (i % 7 == 0 || i % 7 < 3)
                    Assert.IsTrue(events.Count > 0);
                else
                    Assert.IsTrue(events.Count == 0);
                if (i == 1)
                    Assert.IsTrue(events.Contains(EventType.Gold) && events.Count == 4);
            }
        }
    }
}
