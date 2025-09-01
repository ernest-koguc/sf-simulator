using SpawnDev.BlazorJS.JSObjects;

namespace SFSimulator.Frontend;

public class StoreSet<T>(string storeName) where T : IEntity<Guid>
{
    public async Task Put(T entity)
    {
        using var store = await GetStore<Guid, T>(storeName);
        if (entity.Id == default)
        {
            entity.Id = Guid.NewGuid();

        }
        entity.LastModification = DateTime.Now;
        await store.PutAsync(entity);
    }

    public async Task<T> Get(Guid id)
    {
        using var store = await GetStore<Guid, T>(storeName);
        return await store.GetAsync(id);
    }

    public async Task<IEnumerable<T>> Get(Func<T, bool>? filter = null)
    {
        using var store = await GetStore<Guid, T>(storeName);
        return (await store.GetAllAsync()).ToList().Where(filter ?? ((e) => true));
    }

    public async Task Remove(Guid id)
    {
        using var store = await GetStore<Guid, T>(storeName);
        await store.DeleteAsync(id);
    }

    private async Task<IDBObjectStore<TKey, TValue>> GetStore<TKey, TValue>(string storeName)
    {
        using var dbFactory = new IDBFactory();
        using var db = await dbFactory.OpenAsync(DatabaseService.DatabaseName, 3, async upgradeEvent =>
        {
            using var target = upgradeEvent.Target;
            using var db = target.Result;
            using var transaction = target.Transaction!;

            await MigrateDatabase(transaction, upgradeEvent.OldVersion, upgradeEvent.NewVersion);
        });

        using var tran = db.Transaction(storeName, true);
        var store = tran.ObjectStore<TKey, TValue>(storeName);
        return store;
    }

    private async Task MigrateDatabase(IDBTransaction transaction, long oldVersion, long? newVersion)
    {
        if (oldVersion == 0)
        {
            transaction.Db.CreateObjectStore<Guid, SavedResultEntity>(DatabaseService.SavedResultsStoreName, new IDBObjectStoreCreateOptions
            { KeyPath = nameof(SavedResultEntity.Id).ToLower() });
            transaction.Db.CreateObjectStore<Guid, SavedOptionsEntity>(DatabaseService.SavedOptionsStoreName, new IDBObjectStoreCreateOptions
            { KeyPath = nameof(SavedOptionsEntity.Id).ToLower() });

            return;
        }

        if (oldVersion < 3)
        {
            var store = transaction.ObjectStore<Guid, SavedOptionsEntity>(DatabaseService.SavedOptionsStoreName);
            var oldRecords = await store.GetAllAsync();

            // we don't need to provide anything as serialization will automatically populate missing fields and remove obsolote ones
            foreach (var oldRecord in oldRecords.ToList())
            {
                await store.PutAsync(oldRecord);
            }
        }
    }
}
