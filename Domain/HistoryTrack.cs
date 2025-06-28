using Google.Cloud.Firestore;

namespace platejury_api.Domain;

[FirestoreData]
public class HistoryTrack
{
    [FirestoreDocumentId]
    public required string Id { get; set; }
    [FirestoreProperty("userId")]
    public required string UserId { get; set; }
    [FirestoreProperty("userName")]
    public required string UserName { get; set; }
    [FirestoreProperty("trackId")]
    public required string TrackId { get; set; }
    [FirestoreProperty("trackName")]
    public required string TrackName { get; set; }
    [FirestoreProperty("points")]
    public int Points { get; set; }
    [FirestoreProperty("position")]
    public int Position { get; set; }
    [FirestoreProperty("resultDay")]
    public DateTime ResultDay { get; set; }
}