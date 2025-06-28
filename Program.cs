using Google.Cloud.Firestore;
using Microsoft.Extensions.Caching.Memory;
using platejury_api.Domain;

var builder = WebApplication.CreateBuilder(args);

// Add configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Register Firestore
builder.Services.AddSingleton(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var projectId = config["GoogleCloud:ProjectId"];
    return FirestoreDb.Create(projectId);
});

// Register cache
builder.Services.AddMemoryCache();

var app = builder.Build();

// Endpoints
app.MapGet("/history", async (FirestoreDb db, IMemoryCache cache) =>
{
    const string cacheKey = "firestore_history";

    // Check cache
    if (cache.TryGetValue(cacheKey, out List<HistoryTrack>? cachedData))
    {
        return Results.Ok(cachedData);
    }

    var snapshot = await db.Collection("history").GetSnapshotAsync();
    var documents = snapshot.Documents
        .Select(doc => doc.ConvertTo<HistoryTrack>())
        .ToList();

    // Cache result
    var cacheOptions = new MemoryCacheEntryOptions()
        .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));

    cache.Set(cacheKey, documents, cacheOptions);

    return Results.Ok(documents);
});

app.Run();