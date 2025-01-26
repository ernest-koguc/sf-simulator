namespace SFSimulator.Core;

public static class SimulationContextExtensions
{
    public static void PerformAction(this SimulationContext simulationContext, ThirstSimulationOptions thirstOptions, ActionType action)
    {
        switch (action)
        {
            case ActionType.BuyGriffin:
                ChangeMount(simulationContext, thirstOptions, MountType.Griffin);
                break;
            case ActionType.BuyTiger:
                ChangeMount(simulationContext, thirstOptions, MountType.Tiger);
                break;
            case ActionType.SpinMaxTimes:
                ChangeDailySpins(simulationContext, SpinAmountType.Max);
                break;
            case ActionType.SpinOnce:
                ChangeDailySpins(simulationContext, SpinAmountType.OnlyFree);
                break;
            case ActionType.DrinkAllBeers:
                ChangeDailyBeers(simulationContext, 11);
                break;
            case ActionType.DrinkOneBeer:
                ChangeDailyBeers(simulationContext, 1);
                break;
            case ActionType.UpgradeGoldPit:
                UpgradeGoldPit(simulationContext);
                break;
            case ActionType.UpgradeTreasury:
                UpgradeTreasury(simulationContext);
                break;
            case ActionType.UpgradeAcademy:
                UpgradeAcademy(simulationContext);
                break;
            case ActionType.AddOneHydraHead:
                AddOneHydraHead(simulationContext);
                break;
            default:
                throw new NotImplementedException("This schedule action is not supported");
        }
    }

    private static void AddOneHydraHead(SimulationContext simulationContext)
    {
        if (simulationContext.HydraHeads < 20)
            simulationContext.HydraHeads++;
    }

    private static void UpgradeAcademy(SimulationContext simulationContext)
    {
        if (simulationContext.AcademyLevel < 20)
            simulationContext.AcademyLevel++;
    }

    private static void UpgradeTreasury(SimulationContext simulationContext)
    {
        if (simulationContext.TreasuryLevel < 45)
            simulationContext.TreasuryLevel++;
    }
    private static void UpgradeGoldPit(SimulationContext simulationContext)
    {
        if (simulationContext.GoldPitLevel < 100)
            simulationContext.GoldPitLevel++;
    }

    private static void ChangeDailySpins(SimulationContext simulationContext, SpinAmountType spinAmount)
    {
        simulationContext.SpinAmount = spinAmount;
    }

    private static void ChangeDailyBeers(SimulationContext simulationContext, int beers)
    {
        simulationContext.DailyThirst = 100 + beers * 20;
    }

    private static void ChangeMount(SimulationContext simulationContext, ThirstSimulationOptions thirstOptions, MountType mount)
    {
        simulationContext.Mount = mount;
        thirstOptions.Mount = mount;
    }

}