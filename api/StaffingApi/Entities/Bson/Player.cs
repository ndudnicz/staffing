using MongoDB.Bson.Serialization.Attributes;

namespace StaffingApi.Entities.Bson;
    
public class Player: AMyBsonEntity
{
    // public abstract class Position
    // {
    //     public required string LineUpId { get; set; }
    //     public required string Name { get; set; }
    // }
    [BsonElement("name")]
    public required string Name { get; set; }
    [BsonElement("lineUpIds")]
    public required string[] LineUpIds { get; set; } = [];
    // public List<Position> Positions { get; set; } = [];
    
}