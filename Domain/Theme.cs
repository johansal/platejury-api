using Google.Cloud.Firestore;

namespace platejury_api.Domain;

[FirestoreData]
public class Theme
{
    [FirestoreDocumentId]
    public required string Id { get; set; }
    [FirestoreProperty("addedBy")]
    public required string AddedBy { get; set; }
    [FirestoreProperty("name")]
    public required string Name { get; set; }
    [FirestoreProperty("resultDay")]
    public DateTime ResultDay { get; set; }
}