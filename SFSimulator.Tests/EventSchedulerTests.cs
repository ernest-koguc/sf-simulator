using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuestSimulator.Enums;
using QuestSimulator.TavernEvents;
using System.Collections.Generic;

namespace QuestSimulatorTests
{
    [TestClass]
    public class QuestSimulatorTests
    {
        [TestMethod]
        public void GetCurrentEvents_gives_proper_events()
        {
            
            var eventScheduler = new EventScheduler();
            eventScheduler.SetEvent(1, 1);
            for (var i = 1; i<=84; i++)
            {
                var events = eventScheduler.GetCurrentEvents(i);
                if (i % 7 == 0 || i % 7 > 4)
                    Assert.IsTrue(events.Count>0);
                else
                    Assert.IsTrue(events.Count==0);
                if (i == 5)
                    Assert.IsTrue(events.Contains(EventType.EXPERIENCE) && events.Count == 3);
            }


            eventScheduler = new EventScheduler();
            eventScheduler.SetEvent(7, 6);
            for (var i = 1; i <= 84; i++)
            {
                var events = eventScheduler.GetCurrentEvents(i);
                if (i % 7 == 0 || i % 7 <3)
                    Assert.IsTrue(events.Count > 0);
                else
                    Assert.IsTrue(events.Count == 0);
                if (i == 1)
                    Assert.IsTrue(events.Contains(EventType.GOLD) && events.Count == 4);
            }
        }
    }
}
