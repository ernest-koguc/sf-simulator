namespace SFSimulator.Core;
public class UnderworldService(IGameFormulasService gameFormulasService) : IUnderworldService
{
    private bool IsFinished { get; set; }
    private decimal BuildingTime { get; set; } = 0;
    private decimal BuildingTimeAvailable { get; set; } = 0;
    private Action<SimulationContext>? OnBuildingCompleted { get; set; }
    public void Setup(SimulationContext simulationContext)
    {
        if (simulationContext.GoldPitLevel > 15)
        {
            BuildOrder.Clear();
            return;
        }

        BuildOrder = BuildOrder.Where(buildOrder => buildOrder.Building switch
        {
            UnderworldBuildingType.HeartOfDarkness => buildOrder.Level > simulationContext.HeartOfDarknessLevel,
            UnderworldBuildingType.SoulExtractor => buildOrder.Level > simulationContext.SoulExtractorLevel,
            UnderworldBuildingType.GoblinPit => buildOrder.Level > simulationContext.GoblinPitLevel,
            UnderworldBuildingType.UnderworldGate => buildOrder.Level > simulationContext.UnderworldGateLevel,
            UnderworldBuildingType.TortureChamber => buildOrder.Level > simulationContext.TortureChamberLevel,
            UnderworldBuildingType.Adventuromatic => buildOrder.Level > simulationContext.AdventuromaticLevel,
            UnderworldBuildingType.Keeper => buildOrder.Level > simulationContext.KeeperLevel,
            UnderworldBuildingType.TrollBlock => buildOrder.Level > simulationContext.TrollBlockLevel,
            UnderworldBuildingType.Gladiator => buildOrder.Level > simulationContext.GladiatorLevel,
            UnderworldBuildingType.GoldPit => buildOrder.Level > simulationContext.GoldPitLevel,
            _ => throw new ArgumentOutOfRangeException($"Unsupported building type: {buildOrder.Building}")
        }).ToList();
    }

    public void Progress(SimulationContext simulationContext, List<EventType> events)
    {
        if (simulationContext.Level < 125 || simulationContext.GemMineLevel < 10)
        {
            return;
        }

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

            BuildNext(simulationContext);
        }

    }

    private void BuildNext(SimulationContext simulationContext)
    {
        if (BuildOrder.Count == 0)
        {
            if (simulationContext.GoldPitLevel == 100)
            {
                IsFinished = true;
                return;
            }

            BuildingTime = GetBuildingTimeInDays(UnderworldBuildingType.GoldPit, simulationContext.GoldPitLevel + 1, simulationContext.WorkerLevel);
            OnBuildingCompleted = (c) => c.GoldPitLevel++;
            return;
        }

        var nextBuilding = BuildOrder.First();
        BuildOrder.RemoveAt(0);

        BuildingTime = GetBuildingTimeInDays(nextBuilding.Building, nextBuilding.Level, simulationContext.WorkerLevel);
        OnBuildingCompleted = nextBuilding.Building switch
        {
            UnderworldBuildingType.HeartOfDarkness => (c) => c.HeartOfDarknessLevel++,
            UnderworldBuildingType.SoulExtractor => (c) => c.SoulExtractorLevel++,
            UnderworldBuildingType.GoblinPit => (c) => c.GoblinPitLevel++,
            UnderworldBuildingType.UnderworldGate => (c) => c.UnderworldGateLevel++,
            UnderworldBuildingType.TortureChamber => (c) => c.TortureChamberLevel++,
            UnderworldBuildingType.Adventuromatic => (c) => c.AdventuromaticLevel++,
            UnderworldBuildingType.Keeper => (c) => c.KeeperLevel++,
            UnderworldBuildingType.TrollBlock => (c) => c.TrollBlockLevel++,
            UnderworldBuildingType.Gladiator => (c) => c.GladiatorLevel++,
            UnderworldBuildingType.GoldPit => (c) => c.GoldPitLevel++,
            _ => throw new NotImplementedException($"Underworld building of type {nextBuilding.Building} is not implemented"),
        };
    }

    private decimal GetBuildingTimeInDays(UnderworldBuildingType building, int nextBuildingLevel, int workerLevel)
    {
        return gameFormulasService.GetUnderworldBuildingTime(building, nextBuildingLevel, workerLevel) / 60 / 60 / 24;
    }

    private List<UnderworldBuildOrder> BuildOrder =
    [
        new (UnderworldBuildingType.HeartOfDarkness, 1),
        new (UnderworldBuildingType.SoulExtractor, 1),
        new (UnderworldBuildingType.GoblinPit, 1),

        new (UnderworldBuildingType.HeartOfDarkness, 2),
        new (UnderworldBuildingType.UnderworldGate, 1),
        new (UnderworldBuildingType.UnderworldGate, 2),
        new (UnderworldBuildingType.SoulExtractor, 2),

        new (UnderworldBuildingType.HeartOfDarkness, 3),
        new (UnderworldBuildingType.SoulExtractor, 3),
        new (UnderworldBuildingType.UnderworldGate, 3),

        new (UnderworldBuildingType.TortureChamber, 1),

        new (UnderworldBuildingType.HeartOfDarkness, 4),
        new (UnderworldBuildingType.SoulExtractor, 4),
        new (UnderworldBuildingType.UnderworldGate, 4),

        new (UnderworldBuildingType.Gladiator, 1),

        new (UnderworldBuildingType.HeartOfDarkness, 5),
        new (UnderworldBuildingType.SoulExtractor, 5),
        new (UnderworldBuildingType.UnderworldGate, 5),

        new (UnderworldBuildingType.Keeper, 1),
        new (UnderworldBuildingType.Keeper, 2),
        new (UnderworldBuildingType.Keeper, 3),

        new (UnderworldBuildingType.Adventuromatic, 1),

        new (UnderworldBuildingType.TortureChamber, 2),
        new (UnderworldBuildingType.Adventuromatic, 2),

        new (UnderworldBuildingType.Gladiator, 2),
        new (UnderworldBuildingType.Gladiator, 3),

        new (UnderworldBuildingType.TortureChamber, 3),

        new (UnderworldBuildingType.HeartOfDarkness, 6),
        new (UnderworldBuildingType.SoulExtractor, 6),
        new (UnderworldBuildingType.UnderworldGate, 6),

        new (UnderworldBuildingType.Keeper, 4),

        new (UnderworldBuildingType.Gladiator, 4),
        new (UnderworldBuildingType.Gladiator, 5),

        new (UnderworldBuildingType.Adventuromatic, 3),

        new (UnderworldBuildingType.Adventuromatic, 4),
        new (UnderworldBuildingType.TortureChamber, 4),


        new (UnderworldBuildingType.GoldPit, 1),
        new (UnderworldBuildingType.GoldPit, 2),
        new (UnderworldBuildingType.GoldPit, 3),
        new (UnderworldBuildingType.GoldPit, 4),
        new (UnderworldBuildingType.GoldPit, 5),

        new (UnderworldBuildingType.HeartOfDarkness, 7),
        new (UnderworldBuildingType.SoulExtractor, 7),
        new (UnderworldBuildingType.UnderworldGate, 7),

        new (UnderworldBuildingType.Gladiator, 6),

        new (UnderworldBuildingType.Adventuromatic, 5),
        new (UnderworldBuildingType.TortureChamber, 5),
        new (UnderworldBuildingType.Keeper, 5),

        new (UnderworldBuildingType.HeartOfDarkness, 8),
        new (UnderworldBuildingType.SoulExtractor, 8),
        new (UnderworldBuildingType.UnderworldGate, 8),

        new (UnderworldBuildingType.Gladiator, 7),

        new (UnderworldBuildingType.TortureChamber, 6),
        new (UnderworldBuildingType.GoldPit, 6),
        new (UnderworldBuildingType.Keeper, 6),
        new (UnderworldBuildingType.Adventuromatic, 6),

        new (UnderworldBuildingType.HeartOfDarkness, 9),
        new (UnderworldBuildingType.SoulExtractor, 9),
        new (UnderworldBuildingType.UnderworldGate, 9),

        new (UnderworldBuildingType.Gladiator, 8),

        new (UnderworldBuildingType.TortureChamber, 7),
        new (UnderworldBuildingType.GoldPit, 7),
        new (UnderworldBuildingType.Keeper, 7),
        new (UnderworldBuildingType.Adventuromatic, 7),

        new (UnderworldBuildingType.HeartOfDarkness, 10),
        new (UnderworldBuildingType.SoulExtractor, 10),
        new (UnderworldBuildingType.UnderworldGate, 10),

        new (UnderworldBuildingType.Gladiator, 9),

        new (UnderworldBuildingType.TortureChamber, 8),
        new (UnderworldBuildingType.GoldPit, 8),
        new (UnderworldBuildingType.Keeper, 8),
        new (UnderworldBuildingType.Adventuromatic, 8),

        new (UnderworldBuildingType.HeartOfDarkness, 11),
        new (UnderworldBuildingType.SoulExtractor, 11),
        new (UnderworldBuildingType.UnderworldGate, 11),

        new (UnderworldBuildingType.Gladiator, 10),

        new (UnderworldBuildingType.TortureChamber, 9),
        new (UnderworldBuildingType.GoldPit, 9),
        new (UnderworldBuildingType.Keeper, 9),
        new (UnderworldBuildingType.Adventuromatic, 9),

        new (UnderworldBuildingType.HeartOfDarkness, 12),
        new (UnderworldBuildingType.SoulExtractor, 12),
        new (UnderworldBuildingType.UnderworldGate, 12),

        new (UnderworldBuildingType.Gladiator, 11),

        new (UnderworldBuildingType.TortureChamber, 10),
        new (UnderworldBuildingType.GoldPit, 10),
        new (UnderworldBuildingType.Keeper, 10),
        new (UnderworldBuildingType.Adventuromatic, 10),

        new (UnderworldBuildingType.HeartOfDarkness, 13),
        new (UnderworldBuildingType.SoulExtractor, 13),
        new (UnderworldBuildingType.UnderworldGate, 13),

        new (UnderworldBuildingType.Gladiator, 12),

        new (UnderworldBuildingType.TortureChamber, 11),
        new (UnderworldBuildingType.GoldPit, 11),
        new (UnderworldBuildingType.Keeper, 11),
        new (UnderworldBuildingType.Adventuromatic, 11),

        new (UnderworldBuildingType.HeartOfDarkness, 14),
        new (UnderworldBuildingType.SoulExtractor, 14),
        new (UnderworldBuildingType.UnderworldGate, 14),

        new (UnderworldBuildingType.Gladiator, 13),

        new (UnderworldBuildingType.TortureChamber, 12),
        new (UnderworldBuildingType.GoldPit, 12),
        new (UnderworldBuildingType.Keeper, 12),
        new (UnderworldBuildingType.Adventuromatic, 12),

        new (UnderworldBuildingType.HeartOfDarkness, 15),
        new (UnderworldBuildingType.SoulExtractor, 15),
        new (UnderworldBuildingType.UnderworldGate, 15),

        new (UnderworldBuildingType.Gladiator, 14),

        new (UnderworldBuildingType.TortureChamber, 13),
        new (UnderworldBuildingType.GoldPit, 13),
        new (UnderworldBuildingType.Keeper, 13),
        new (UnderworldBuildingType.Adventuromatic, 13),

        new (UnderworldBuildingType.Gladiator, 15),

        new (UnderworldBuildingType.TortureChamber, 14),
        new (UnderworldBuildingType.GoldPit, 14),
        new (UnderworldBuildingType.Keeper, 14),
        new (UnderworldBuildingType.Adventuromatic, 14),

        new (UnderworldBuildingType.TortureChamber, 15),
        new (UnderworldBuildingType.GoldPit, 15),
        new (UnderworldBuildingType.Keeper, 15),
        new (UnderworldBuildingType.Adventuromatic, 15),

        new (UnderworldBuildingType.GoblinPit, 2),
        new (UnderworldBuildingType.GoblinPit, 3),
        new (UnderworldBuildingType.GoblinPit, 4),
        new (UnderworldBuildingType.GoblinPit, 5),
        new (UnderworldBuildingType.GoblinPit, 6),
        new (UnderworldBuildingType.GoblinPit, 7),
        new (UnderworldBuildingType.GoblinPit, 8),
        new (UnderworldBuildingType.GoblinPit, 9),
        new (UnderworldBuildingType.GoblinPit, 10),
        new (UnderworldBuildingType.GoblinPit, 11),
        new (UnderworldBuildingType.GoblinPit, 12),
        new (UnderworldBuildingType.GoblinPit, 13),
        new (UnderworldBuildingType.GoblinPit, 14),
        new (UnderworldBuildingType.GoblinPit, 15),

        new (UnderworldBuildingType.TrollBlock, 1),
        new (UnderworldBuildingType.TrollBlock, 2),
        new (UnderworldBuildingType.TrollBlock, 3),
        new (UnderworldBuildingType.TrollBlock, 4),
        new (UnderworldBuildingType.TrollBlock, 5),
        new (UnderworldBuildingType.TrollBlock, 6),
        new (UnderworldBuildingType.TrollBlock, 7),
        new (UnderworldBuildingType.TrollBlock, 8),
        new (UnderworldBuildingType.TrollBlock, 9),
        new (UnderworldBuildingType.TrollBlock, 10),
        new (UnderworldBuildingType.TrollBlock, 11),
        new (UnderworldBuildingType.TrollBlock, 12),
        new (UnderworldBuildingType.TrollBlock, 13),
        new (UnderworldBuildingType.TrollBlock, 14),
        new (UnderworldBuildingType.TrollBlock, 15),
    ];
}

public record UnderworldBuildOrder(UnderworldBuildingType Building, int Level);
