using SFSimulator.Core;
using SpawnDev.BlazorJS.JSObjects;

namespace SFSimulator.Frontend;

public class DatabaseService
{
    private const string DatabaseName = "SFSimulator";
    private const string SavedResultsStoreName = "SimulationResults";

    public async Task SaveResult(string name, SimulationResult simulationResult)
    {

        var savedResultEntity = new SavedResultEntity
        {
            Name = name,
            Id = Guid.NewGuid(),
            LastModification = DateTime.Now,
            Started = DateOnly.FromDateTime(DateTime.Now),
            Finished = DateOnly.FromDateTime(DateTime.Now).AddDays(simulationResult.Days),
            Days = simulationResult.Days,
            BeforeSimulation = simulationResult.BeforeSimulation,
            AfterSimulation = simulationResult.AfterSimulation,
            SimulatedDays = simulationResult.SimulatedDays,
            Achievements = simulationResult.Achievements,
        };
        using var store = await GetStore<Guid, SavedResultEntity>(SavedResultsStoreName);
        await store.PutAsync(savedResultEntity);
    }

    public async Task<List<SavedResultEntity>> GetSavedResults()
    {
        using var store = await GetStore<Guid, SavedResultEntity>(SavedResultsStoreName);

        return (await store.GetAllAsync()).ToList();
    }

    private async Task<IDBObjectStore<TKey, TValue>> GetStore<TKey, TValue>(string storeName)
    {
        using var dbFactory = new IDBFactory();
        using var db = await dbFactory.OpenAsync(DatabaseName, 1, upgradeEvent =>
        {
            using var target = upgradeEvent.Target;
            using var db = target.Result;

            if (upgradeEvent.NewVersion == 1)
            {
                using var store = db.CreateObjectStore<Guid, SavedResultEntity>(storeName, new IDBObjectStoreCreateOptions
                { KeyPath = nameof(SavedResultEntity.Id).ToLower() });
            }
        });

        using var tran = db.Transaction(storeName, true);
        var store = tran.ObjectStore<TKey, TValue>(storeName);
        return store;
    }
}
