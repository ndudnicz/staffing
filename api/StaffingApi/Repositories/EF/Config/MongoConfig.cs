using MongoDB.Driver;

namespace StaffingApi.Repositories.EF.Config;

public class MongoConfig(IMongoClient client, string databaseName): IMongoConfig
{
    public IMongoClient MongoClient => client;
    public string DatabaseName => databaseName;
}