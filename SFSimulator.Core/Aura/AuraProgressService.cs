namespace SFSimulator.Core;

public class AuraProgressService : IAuraProgressService
{
    public void IncreaseAuraProgress(SimulationContext simulationContext, bool isToiletEvent)
    {
        if (simulationContext.Level < 100)
            return;

        if (simulationContext.Aura == 66)
            return;

        var points = simulationContext.AuraStrategy switch
        {
            AuraProgressStrategyType.OnlyNormalItems => 25,
            AuraProgressStrategyType.OnlyEpicItems => 50,
            AuraProgressStrategyType.NormalItemsAndEpicDuringEvent when isToiletEvent => 50,
            AuraProgressStrategyType.NormalItemsAndEpicDuringEvent => 25,
            _ => throw new InvalidOperationException($"Aura progress strategy for {simulationContext.AuraStrategy} is not implemented")
        };

        if (isToiletEvent)
            points *= 2;

        simulationContext.AuraFillLevel += points;

        if (simulationContext.AuraFillLevel >= GetAuraFillCapacity(simulationContext.Aura))
        {
            simulationContext.AuraFillLevel = 0;
            simulationContext.Aura++;
        }
    }

    private int GetAuraFillCapacity(int auraLevel)
        => auraLevel switch
        {
            0 => 100,
            1 => 150,
            2 => 200,
            3 => 250,
            4 => 300,
            5 => 350,
            6 => 400,
            7 => 450,
            8 => 500,
            9 => 550,
            10 => 600,
            11 => 650,
            12 => 700,
            13 => 750,
            14 => 800,
            15 => 850,
            16 => 900,
            17 => 950,
            >= 18 => 1000,
            _ => throw new InvalidOperationException($"Aura level {auraLevel} should not be possible")
        };
}
