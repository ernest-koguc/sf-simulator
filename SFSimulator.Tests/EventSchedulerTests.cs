using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFSimulator.Core;
using System.Linq;

namespace SFSimulator.Tests;

[TestClass]
public class EventSchedulerTests
{


    [TestMethod]
    public void GetCurrentEvents_gives_proper_events()
    {

        var eventScheduler = new Scheduler();
        eventScheduler.SetStartingPoint(1, 1);
        for (var i = 1; i <= 84; i++)
        {
            var events = eventScheduler.GetEvents();
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
            var events = eventScheduler.GetEvents();
            if (i % 7 == 0 || i % 7 < 3)
                Assert.IsTrue(events.Count > 0);
            else
                Assert.IsTrue(events.Count == 0);
            if (i == 1)
                Assert.IsTrue(events.Contains(EventType.Gold) && events.Count == 4);
        }
    }

    [TestMethod]
    public void Year2024Cycle_has_correct_schedule()
    {
        var scheduler = new Scheduler();

        var schedule = scheduler.Year2024Cycle;
        var keys = schedule.Keys.ToList();
        var values = schedule.Values.ToList();

        Assert.AreEqual(364, schedule.Count);
        Assert.IsTrue(keys.GroupBy(k => k.Week).All(k => k.Key != 1 && k.Key != 53 ? k.Count() == 7 : true), "All weeks should have 7 days (except first and last week)");
        Assert.IsTrue(keys.GroupBy(k => k.Week).All(k => k.Count() == k.Distinct().Count()), "Days should be unique within week");
        foreach (var keyValue in schedule)
        {
            Assert.IsTrue(keyValue.Value.Count == keyValue.Value.Distinct().Count(), $"Duplicate events for week {keyValue.Key.Week}, day {keyValue.Key.Day}");
        }
    }
}