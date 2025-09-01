namespace SFSimulator.Core;

public class AuraProgressService : IAuraProgressService
{
    public void IncreaseAuraProgress(SimulationContext simulationContext, bool isToiletEvent)
    {
        if (simulationContext.Level < 100)
            return;

        if (simulationContext.Aura == 400)
            return;

        if (simulationContext.Aura == 0)
        {
            simulationContext.Aura = 1;
        }

        var points = simulationContext.AuraStrategy switch
        {
            AuraProgressStrategyType.OnlyNormalItems => 25,
            AuraProgressStrategyType.OnlyEpicItems => 50,
            AuraProgressStrategyType.NormalItemsAndEpicDuringEvent when isToiletEvent => 50,
            AuraProgressStrategyType.NormalItemsAndEpicDuringEvent => 25,
            _ => throw new InvalidOperationException($"Aura progress strategy for {simulationContext.AuraStrategy} is not implemented")
        };

        // Double points and double items for sacrifice 
        if (isToiletEvent)
            points *= 4;

        simulationContext.AuraFillLevel += points;

        var maxCapacity = GetAuraFillCapacity(simulationContext.Aura);
        while (simulationContext.AuraFillLevel >= maxCapacity)
        {
            simulationContext.AuraFillLevel -= maxCapacity;
            simulationContext.Aura++;
            maxCapacity = GetAuraFillCapacity(simulationContext.Aura);
        }
    }

    private int GetAuraFillCapacity(int auraLevel)
        => auraLevel switch
        {
            < 1 => throw new InvalidOperationException("Aura level should not be below 1"),
            < 97 => 60 + (auraLevel - 1) / 8 * 20,
            >= 97 => 300,
        };
}