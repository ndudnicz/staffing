using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using StaffingApi.Entities.Dto;

namespace StaffingApi.Entities.Bson;
    
public class Player: AMyBsonEntity
{
    [BsonElement("name")]
    public required string Name { get; set; }
    [BsonElement("lineUpIds")]
    public required string[] LineUpIds { get; set; } = [];
    
    public static Player FromPlayerDto(PlayerDto dto)
    {
        return new Player
        {
            _id = ObjectId.Parse(dto.Id),
            Name = dto.Name,
            LineUpIds = dto.LineUpIds.ToArray(),
            Created = dto.Created,
            Modified = dto.Modified
        };
    }
    
    public static Player FromCreatePlayerDto(CreatePlayerDto dto)
    {
        return new Player
        {
            Name = dto.Name,
            LineUpIds = dto.LineUpIds.ToArray()
        };
    }
}