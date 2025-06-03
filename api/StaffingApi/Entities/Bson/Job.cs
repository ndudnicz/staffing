namespace StaffingApi.Entities.Bson;

public class Job: AMyBsonEntity
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string LineUpId { get; set; }
    public required Guid TacticId { get; set; }
    public required Guid PhaseId { get; set; }
}