namespace StaffingApi.Entities;

public class Tactic
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required List<Phase> Phases { get; set; } = [];
}