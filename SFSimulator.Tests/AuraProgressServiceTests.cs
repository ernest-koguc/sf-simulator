using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFSimulator.Core;

namespace SFSimulator.Tests;

[TestClass]
public class AuraProgressServiceTests
{
    [TestMethod]
    public void IncreaseAuraProgress_doesnot_increase_aura_below_level_100()
    {
        var service = DependencyProvider.Get<IAuraProgressService>();
        var context = new SimulationContext { Level = 99, Aura = 0, AuraFillLevel = 0, AuraStrategy = AuraProgressStrategyType.OnlyNormalItems };

        service.IncreaseAuraProgress(context, false);

        Assert.AreEqual(0, context.AuraFillLevel);
        Assert.AreEqual(0, context.Aura);
    }

    [TestMethod]
    public void IncreaseAuraProgress_doesnot_exceed_aura_limit()
    {
        var service = DependencyProvider.Get<IAuraProgressService>();
        var context = new SimulationContext { Level = 100, Aura = 66, AuraFillLevel = 999, AuraStrategy = AuraProgressStrategyType.OnlyNormalItems };

        service.IncreaseAuraProgress(context, false);

        Assert.AreEqual(66, context.Aura);
    }

    [TestMethod]
    [DataRow(AuraProgressStrategyType.OnlyNormalItems, 25, false)]
    [DataRow(AuraProgressStrategyType.OnlyNormalItems, 50, true)]
    [DataRow(AuraProgressStrategyType.OnlyEpicItems, 50, false)]
    [DataRow(AuraProgressStrategyType.OnlyEpicItems, 100, true)]
    [DataRow(AuraProgressStrategyType.NormalItemsAndEpicDuringEvent, 25, false)]
    [DataRow(AuraProgressStrategyType.NormalItemsAndEpicDuringEvent, 100, true)]
    public void IncreaseAuraProgress_gives_correct_mana_amount(AuraProgressStrategyType strategy, int expectedFillLevel, bool isToiletEvent)
    {
        var service = DependencyProvider.Get<IAuraProgressService>();
        var context = new SimulationContext { Level = 100, Aura = 20, AuraStrategy = strategy };

        service.IncreaseAuraProgress(context, isToiletEvent);

        Assert.AreEqual(expectedFillLevel, context.AuraFillLevel);
    }

    [TestMethod]
    public void IncreaseAuraProgress_increases_aura_level_correctly()
    {
        var service = DependencyProvider.Get<IAuraProgressService>();
        var context = new SimulationContext { Level = 100, Aura = 20, AuraFillLevel = 975, AuraStrategy = AuraProgressStrategyType.OnlyNormalItems };

        service.IncreaseAuraProgress(context, false);

        Assert.AreEqual(0, context.AuraFillLevel);
        Assert.AreEqual(21, context.Aura);
    }
}
