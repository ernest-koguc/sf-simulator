namespace SFSimulator.Core;

public class FortressService(IGameFormulasService gameFormulasService) : IFortressService
{
    private bool IsFinished { get; set; }
    private decimal BuildingTime { get; set; } = 0;
    private decimal BuildingTimeAvailable { get; set; } = 0;
    private Action<SimulationContext>? OnBuildingCompleted { get; set; }
    public void Progress(SimulationContext simulationContext, List<EventType> events)
    {
        DrHouse.Differential($"Fortress: {simulationContext.FortressLevel}, Workers: {simulationContext.WorkerLevel}, GemMine: {simulationContext.GemMineLevel}, GoldPit: {simulationContext.GoldPitLevel}, Academy: {simulationContext.AcademyLevel}, Treasury: {simulationContext.TreasuryLevel}");
        if (simulationContext.Level < 25)
            return;

        if (simulationContext.FortressLevel == 0)
            simulationContext.FortressLevel = 1;

        BuildingTimeAvailable = 1;

        while (BuildingTimeAvailable > 0 && !IsFinished)
        {

            if (BuildingTime > BuildingTimeAvailable)
            {
                BuildingTime -= BuildingTimeAvailable;
                return;
            }

            BuildingTimeAvailable -= BuildingTime;
            OnBuildingCompleted?.Invoke(simulationContext);
            BuildingTime = 0;

            // We should unlock underworld as soon as possible
            if (simulationContext.Level >= 125 && simulationContext.GemMineLevel < 10)
                PushToGemMine10(simulationContext);
            else
            {
                BuildInRegularOrder(simulationContext);
            }
        }
    }

    private void BuildInRegularOrder(SimulationContext simulationContext)
    {
        if (simulationContext.FortressLevel > simulationContext.WorkerLevel && simulationContext.WorkerLevel < 15)
        {
            BuildingTime = GetBuildingTimeInDays(FortressBuildingType.Worker, simulationContext.WorkerLevel + 1, simulationContext.WorkerLevel);
            OnBuildingCompleted = (c) => c.WorkerLevel++;
        }
        else if (simulationContext.FortressLevel >= 15 && simulationContext.WorkerLevel >= 15
            && simulationContext.AcademyLevel < simulationContext.FortressLevel)
        {
            BuildingTime = GetBuildingTimeInDays(FortressBuildingType.Academy, simulationContext.AcademyLevel + 1, simulationContext.WorkerLevel);
            OnBuildingCompleted = (c) => c.AcademyLevel++;
        }
        else if (simulationContext.TreasuryLevel < 10 && simulationContext.WorkerLevel == 15)
        {
            BuildingTime = GetBuildingTimeInDays(FortressBuildingType.Treasury, simulationContext.TreasuryLevel + 1, simulationContext.WorkerLevel);
            OnBuildingCompleted = (c) => c.TreasuryLevel++;
        }
        else if (simulationContext.FortressLevel < 20)
        {
            BuildingTime = GetBuildingTimeInDays(FortressBuildingType.Fortress, simulationContext.FortressLevel + 1, simulationContext.WorkerLevel);
            OnBuildingCompleted = (c) => c.FortressLevel++;
        }
        else if (simulationContext.TreasuryLevel < 45)
        {
            BuildingTime = GetBuildingTimeInDays(FortressBuildingType.Treasury, simulationContext.TreasuryLevel + 1, simulationContext.WorkerLevel);
            OnBuildingCompleted = (c) => c.TreasuryLevel++;
        }
        else if (simulationContext.GemMineLevel < 100)
        {
            BuildingTime = GetBuildingTimeInDays(FortressBuildingType.GemMine, simulationContext.GemMineLevel + 1, simulationContext.WorkerLevel);
            OnBuildingCompleted = (c) => c.GemMineLevel++;
        }
        else
        {
            IsFinished = true;
        }
    }

    private void PushToGemMine10(SimulationContext simulationContext)
    {
        if (simulationContext.FortressLevel < 10)
        {
            // upgrade to fort 10 asap
            BuildingTime = GetBuildingTimeInDays(FortressBuildingType.Fortress, simulationContext.FortressLevel + 1, simulationContext.WorkerLevel);
            OnBuildingCompleted = (c) => c.FortressLevel++;
        }
        else
        {
            BuildingTime = GetBuildingTimeInDays(FortressBuildingType.GemMine, simulationContext.GemMineLevel + 1, simulationContext.WorkerLevel);
            OnBuildingCompleted = (c) => c.GemMineLevel++;
        }
    }

    private decimal GetBuildingTimeInDays(FortressBuildingType building, int nextBuildingLevel, int workerLevel)
    {
        return gameFormulasService.GetFortressBuildingTime(building, nextBuildingLevel, workerLevel) / 60 / 60 / 24;
    }
}
