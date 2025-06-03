namespace StaffingApi.Entities;

public class Phase
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
}