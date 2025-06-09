using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using StaffingApi.Entities.Dto;

namespace StaffingApi.Entities.Bson;

public class LineUp: AMyBsonEntity
{
    [BsonElement("name")]
    public required string Name { get; set; }
    [BsonElement("playerIds")]
    public required string[] PlayerIds { get; set; } = [];
    
    public static LineUp FromLineUpDto(LineUpDto dto)
    {
        return new LineUp
        {
            _id = ObjectId.Parse(dto.Id),
            Name = dto.Name,
            PlayerIds = dto.PlayerIds.ToArray(),
            Created = dto.Created,
            Modified = dto.Modified
        };
    }
    
    public static LineUp FromCreateLineUpDto(CreateLineUpDto dto)
    {
        return new LineUp
        {
            Name = dto.Name,
            PlayerIds = dto.PlayerIds.ToArray()
        };
    }
}