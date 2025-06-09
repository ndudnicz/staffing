namespace StaffingApi.Entities.Bson;
    
public class CreatePlayerDto
{
    public string Name { get; set; }
    public string[] LineUpIds { get; set; } = [];
}