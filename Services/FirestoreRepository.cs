using Google.Cloud.Firestore;
using Microsoft.Extensions.Caching.Memory;
using platejury_api.Domain;

namespace platejury_api.Services;

public class FirestoreRepository<T>(FirestoreDb db, IMemoryCache cache, CacheSettings config) where T : class
{
    public async Task<List<T>> GetCollectionAsync(string collectionName)
    {
        var cacheKey = $"firestore_{collectionName}";

        if (cache.TryGetValue(cacheKey, out List<T>? cached) && cached != null)
        {
            return cached;
        }
            
        var snapshot = await db.Collection(collectionName).GetSnapshotAsync();
        var results = snapshot.Documents.Select(doc => doc.ConvertTo<T>()).ToList();

        var expiration = TimeSpan.FromMinutes(config.AbsoluteExpirationMinutes);
        var options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(expiration);

        cache.Set(cacheKey, results, options);
        return results;
    }

}