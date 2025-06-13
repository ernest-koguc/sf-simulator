namespace SFSimulator.Frontend;

public class DatabaseService
{
    public const string DatabaseName = "SFSimulator";
    public const string SavedResultsStoreName = "SimulationResults";
    public const string SavedOptionsStoreName = "SimulationOptions";

    public StoreSet<SavedResultEntity> SavedResults { get; } = new StoreSet<SavedResultEntity>(SavedResultsStoreName);
    public StoreSet<SavedOptionsEntity> SavedOptions { get; } = new StoreSet<SavedOptionsEntity>(SavedOptionsStoreName);
}
