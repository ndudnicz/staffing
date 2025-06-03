using MongoDB.Driver;

namespace StaffingApi.Repositories.EF.Config;

public interface IMongoConfig
{
    public IMongoClient MongoClient { get; }
    public string DatabaseName { get; }
}