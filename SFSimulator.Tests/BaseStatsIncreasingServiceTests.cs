using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFSimulator.Core;
using System;

namespace SFSimulator.Tests;

[TestClass]
public class BaseStatsIncreasingServiceTests
{
    [TestMethod]
    [DataRow(BaseStatsIncreaseStrategyType.Keep_50_50_until_same_cost_then_60_40, 1.0)]
    [DataRow(BaseStatsIncreaseStrategyType.Ratio_60_40, 1.5)]
    [DataRow(BaseStatsIncreaseStrategyType.Ratio_50_50, 1.0)]
    public void IncreaseBaseStats_Increases_base_stats_according_to_strategy(BaseStatsIncreaseStrategyType strategy, double expectedRatio)
    {
        var service = DependencyProvider.Get<IBaseStatsIncreasingService>();
        var simulationContext = new SimulationContext();
        simulationContext.BaseStatsIncreaseStrategy = strategy;
        simulationContext.Gold = 10_000_000;

        service.IncreaseBaseStats(simulationContext);

        var actualRatio = (double)simulationContext.BaseMainAttribute / simulationContext.BaseConstitution;
        var difference = Math.Abs(expectedRatio - actualRatio);

        Assert.IsTrue(difference < 0.01, $"Expected ratio: {expectedRatio}, actual ratio: {actualRatio}");
    }

    [TestMethod]
    public void IncreaseBaseStats_Changes_ratio_after_gold_cap_if_used_as_strategy()
    {
        var service = DependencyProvider.Get<IBaseStatsIncreasingService>();
        var simulationContext = new SimulationContext();
        simulationContext.BaseStatsIncreaseStrategy = BaseStatsIncreaseStrategyType.Keep_50_50_until_same_cost_then_60_40;
        simulationContext.Gold = 11_000_000_000;

        service.IncreaseBaseStats(simulationContext);

        var diff = simulationContext.BaseMainAttribute - simulationContext.BaseConstitution;
        Assert.IsTrue(diff > 1, "Expected main attribute to be higher by at least two than constitution");
    }

    [TestMethod]
    [DataRow(50_000_000_000)]
    [DataRow(100_000_000_000)]
    [DataRow(200_000_000_000)]
    public void IncreaseBaseStats_Change_ratio_strategy_doesnt_exceed_60_40_ratio(double gold)
    {
        var service = DependencyProvider.Get<IBaseStatsIncreasingService>();
        var simulationContext = new SimulationContext();
        simulationContext.BaseStatsIncreaseStrategy = BaseStatsIncreaseStrategyType.Keep_50_50_until_same_cost_then_60_40;
        simulationContext.Gold = (decimal)gold;

        service.IncreaseBaseStats(simulationContext);

        var actualRatio = (double)simulationContext.BaseMainAttribute / simulationContext.BaseConstitution;
        var expectedRatio = 1.5;
        var difference = Math.Abs(expectedRatio - actualRatio);

        Assert.IsTrue(difference < 0.01, $"Expected ratio: {expectedRatio}, actual ratio: {actualRatio}");
    }
}
