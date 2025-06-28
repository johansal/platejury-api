using Google.Cloud.Firestore;

var builder = WebApplication.CreateBuilder(args);

// Add configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Register Firestore
// Register FirestoreDb as a singleton service
builder.Services.AddSingleton(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var projectId = config["GoogleCloud:ProjectId"];
    return FirestoreDb.Create(projectId);
});

var app = builder.Build();

// Configure firestore
var db = FirestoreDb.Create(builder.Configuration["GoogleCloud:ProjectId"]);

// Endpoints
app.MapGet("/history", async () =>
{
    var collection = db.Collection("history");
    var snapshot = await collection.GetSnapshotAsync();
    var documents = snapshot.Documents.Select(doc => doc.ToDictionary()).ToList();

    return Results.Ok(documents);
});

app.Run();
