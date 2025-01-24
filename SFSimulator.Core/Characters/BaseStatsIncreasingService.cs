namespace SFSimulator.Core;
public class BaseStatsIncreasingService(ICurves curves)
{
    public void IncreaseBaseStats(SimulationContext simulationContext)
    {
        switch (simulationContext.BaseStatsIncreaseStrategy)
        {
            case BaseStatsIncreaseStrategyType.Keep_50_50_until_same_cost_then_60_40:
                if (simulationContext.BaseMainAttribute < 3155 || simulationContext.BaseConstitution < 3155)
                {
                    IncreaseBaseStats(1, simulationContext, 3155, 3155);
                    if (simulationContext.BaseMainAttribute >= 3155 && simulationContext.BaseConstitution >= 3155)
                    {
                        IncreaseBaseStats(1.5M, simulationContext);
                    }
                }
                else
                {
                    IncreaseBaseStats(1.5M, simulationContext);
                }
                break;
            case BaseStatsIncreaseStrategyType.Ratio_60_40:
                IncreaseBaseStats(1.5M, simulationContext);
                break;
            case BaseStatsIncreaseStrategyType.Ratio_50_50:
                IncreaseBaseStats(1, simulationContext);
                break;
        }
    }

    private void IncreaseBaseStats(decimal mainConRatio, SimulationContext simulationContext, int? mainAttributeLimit = null, int? conAttributeLimit = null)
    {
        var haveUpgraded = true;
        while (haveUpgraded)
        {
            haveUpgraded = false;
            var currentRatio = 0M;

            if (simulationContext.BaseMainAttribute == 0 && simulationContext.BaseConstitution == 0)
            {
                currentRatio = 1;
            }
            else if (simulationContext.BaseConstitution != 0)
            {
                currentRatio = (decimal)simulationContext.BaseMainAttribute / simulationContext.BaseConstitution;
            }
            else
            {
                currentRatio = decimal.MaxValue;
            }

            if (currentRatio <= mainConRatio && (mainAttributeLimit == null || simulationContext.BaseMainAttribute < mainAttributeLimit))
            {
                var goldCost = GetBaseStatsIncreaseCost(simulationContext.BaseMainAttribute);
                if (goldCost <= simulationContext.Gold)
                {
                    simulationContext.SetBaseMainAttribute(simulationContext.BaseMainAttribute + 1);
                    simulationContext.Gold -= goldCost;
                    haveUpgraded = true;
                }
            }
            else if (conAttributeLimit == null || simulationContext.BaseConstitution < conAttributeLimit)
            {
                var goldCost = GetBaseStatsIncreaseCost(simulationContext.BaseConstitution);
                if (goldCost <= simulationContext.Gold)
                {
                    simulationContext.BaseConstitution++;
                    simulationContext.Gold -= goldCost;
                    haveUpgraded = true;
                }
            }
        }
    }

    private decimal GetBaseStatsIncreaseCost(int attributeLevel)
    {
        if (attributeLevel >= 3155)
        {
            return 10_000_000;
        }

        var cost = 0M;
        for (var i = 0; i < 5; i++)
        {
            var num = (int)Math.Floor(1 + (attributeLevel + i) / 5M);
            cost = num >= 800 ? 5000000000 : cost + curves.GoldCurve[num];
        }
        cost = 5 * Math.Floor(Math.Floor(cost / 5) / 5) / 100;
        cost = cost < 10 ? cost : Math.Min(1E7M, Math.Floor(cost));
        return cost;
    }
}

