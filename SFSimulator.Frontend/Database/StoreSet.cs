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
        using var db = await dbFactory.OpenAsync(DatabaseService.DatabaseName, 1, upgradeEvent =>
        {
            using var target = upgradeEvent.Target;
            using var db = target.Result;

            if (upgradeEvent.NewVersion == 1)
            {
                db.CreateObjectStore<Guid, SavedResultEntity>(DatabaseService.SavedResultsStoreName, new IDBObjectStoreCreateOptions
                { KeyPath = nameof(SavedResultEntity.Id).ToLower() });
                db.CreateObjectStore<Guid, SavedOptionsEntity>(DatabaseService.SavedOptionsStoreName, new IDBObjectStoreCreateOptions
                { KeyPath = nameof(SavedOptionsEntity.Id).ToLower() });
            }
        });

        using var tran = db.Transaction(storeName, true);
        var store = tran.ObjectStore<TKey, TValue>(storeName);
        return store;
    }
}
