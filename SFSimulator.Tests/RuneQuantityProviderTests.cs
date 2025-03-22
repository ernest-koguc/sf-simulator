using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFSimulator.Core;
using System.Linq;

namespace SFSimulator.Tests;

[TestClass]
public class RuneQuantityProviderTests
{
    [TestMethod]
    public void IncreaseRuneQuantity_increases_rune_quantity_each_day()
    {
        var service = DependencyProvider.Get<IRuneQuantityProvider>();
        var simulationContext = new SimulationContext();
        simulationContext.RuneQuantity = 0;

        service.Setup(simulationContext, Enumerable.Range(1, 100).Select(i => new DayRuneQuantity(i, i)).ToList());

        for (var i = 1; i <= 100; i++)
        {
            service.IncreaseRuneQuantity(simulationContext, i);
            Assert.AreEqual(i, simulationContext.RuneQuantity);
        }
    }

    [TestMethod]
    public void IncreaseRuneQuantity_truncates_quantities_that_are_already_obtained()
    {
        var service = DependencyProvider.Get<IRuneQuantityProvider>();
        var simulationContext = new SimulationContext();
        simulationContext.RuneQuantity = 70;

        service.Setup(simulationContext, Enumerable.Range(1, 100).Select(i => new DayRuneQuantity(i, i)).ToList());

        for (var i = 1; i <= 30; i++)
        {
            service.IncreaseRuneQuantity(simulationContext, i);
            Assert.AreEqual(70 + i, simulationContext.RuneQuantity);
        }
    }
}
