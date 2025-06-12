using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace StaffingApi.Entities.Bson;

public abstract class MyBsonEntity
{
    protected MyBsonEntity() {}
    [Key]
    [BsonId]
    [BsonElement("_id")]
    public ObjectId _id { get; set; }
    public string Id => _id.ToString();
    [BsonElement("created")]
    public DateTime Created { get; set; }
    [BsonElement("modified")]
    public DateTime? Modified { get; set; }
}