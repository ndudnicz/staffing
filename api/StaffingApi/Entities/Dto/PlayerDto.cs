using MongoDB.Bson.Serialization.Attributes;

namespace StaffingApi.Entities.Bson;
    
public class PlayerDto
{
    public static PlayerDto? FromPlayer(Player? player)
    {
        if (player == null) return null;
        return new PlayerDto
        {
            Id = player.Id,
            Name = player.Name,
            Created = player.Created,
            Modified = player.Modified,
            LineUpIds = player.LineUpIds.ToArray()
        };
    }
    
    public string Id { get; set; }
    public string Name { get; set; }
    public string[] LineUpIds { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Modified { get; set; }
}