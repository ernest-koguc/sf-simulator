using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFSimulator.Core;
using System.Linq;

namespace SFSimulator.Tests;

[TestClass]
public class EventSchedulerTests
{
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