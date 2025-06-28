using Google.Cloud.Firestore;

namespace platejury_api.Domain;

[FirestoreData]
public class Votes
{
    [FirestoreDocumentId]
    public required string Id { get; set; }
    [FirestoreProperty("voterId")]
    public required string VoterId { get; set; }
    [FirestoreProperty("votedTracks")]
    public required List<Dictionary<string, string>> VotedTracks { get; set; }
    [FirestoreProperty("resultDay")]
    public DateTime ResultDay { get; set; }
}