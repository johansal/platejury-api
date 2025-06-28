using Google.Cloud.Firestore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using platejury_api.Domain;
using platejury_api.Services;

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

// Register cache settings from config
builder.Services.Configure<CacheSettings>(
    builder.Configuration.GetSection("CacheSettings"));
builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<CacheSettings>>().Value);

// Register repository service
builder.Services.AddScoped(typeof(FirestoreRepository<>));

var app = builder.Build();

// Endpoints
app.MapGet("/history", async (FirestoreRepository<HistoryTrack> repo) =>
{
    var documents = await repo.GetCollectionAsync("history");
    return Results.Ok(documents);
});

app.MapGet("/votes", async (FirestoreRepository<Votes> repo) =>
{
    var documents = await repo.GetCollectionAsync("votes");
    return Results.Ok(documents);
});

app.Run();