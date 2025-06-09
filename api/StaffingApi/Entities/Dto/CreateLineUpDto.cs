namespace StaffingApi.Entities.Dto;
    
public class CreateLineUpDto
{
    public string Name { get; set; }
    public string[] PlayerIds { get; set; } = [];
}