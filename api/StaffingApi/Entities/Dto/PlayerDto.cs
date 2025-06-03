using MongoDB.Bson.Serialization.Attributes;

namespace StaffingApi.Entities.Bson;
    
public class PlayerDto
{
    public static PlayerDto? FromPlayer(Player? player)
    {
        if (player == null)
            return null;
        return new PlayerDto
        {
            Id = player.Id,
            Name = player.Name,
            Created = player.Created,
            Modified = player.Modified
        };
    }
    // public abstract class Position
    // {
    //     public required string LineUpId { get; set; }
    //     public required string Name { get; set; }
    // }
    public required string Id { get; set; }
    public required string Name { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Modified { get; set; }
    // public List<Position> Positions { get; set; } = [];
    
}