using MongoDB.Bson.Serialization.Attributes;

namespace StaffingApi.Entities.Bson;

public class LineUp: AMyBsonEntity
{
    [BsonElement("name")]
    public required string Name { get; set; }
    // public required List<Tactic> Tactics { get; set; } = [];
    // public required List<string> PlayerIds { get; set; } = [];
}