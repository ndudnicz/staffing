using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StaffingApi.Entities.Bson;

public abstract class AMyBsonEntity
{
    protected AMyBsonEntity() {}
    [BsonId]
    [BsonElement("_id")]
    public ObjectId _id { get; set; }
    public string Id => _id.ToString();
    [BsonElement("created")]
    public DateTime Created { get; set; }
    [BsonElement("modified")]
    public DateTime? Modified { get; set; }
}