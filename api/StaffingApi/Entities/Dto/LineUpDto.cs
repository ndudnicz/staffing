using MongoDB.Bson.Serialization.Attributes;
using StaffingApi.Entities.Bson;

namespace StaffingApi.Entities.Dto;
    
public class LineUpDto
{
    private LineUpDto() {}

    public static LineUpDto? FromLineUp(LineUp? lineUp)
    {
        if (lineUp == null) return null;
        return new LineUpDto
        {
            Id = lineUp.Id,
            Name = lineUp.Name,
            Created = lineUp.Created,
            Modified = lineUp.Modified
        };
    }

    public static IEnumerable<LineUpDto> FromLineUps(IEnumerable<LineUp> lineUps)
    {
        return lineUps.Select(FromLineUp).Where(x => x != null).Cast<LineUpDto>();
    }

    public string Id { get; set; }
    public string Name { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Modified { get; set; }
}