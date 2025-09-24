namespace MongoDBConnectorLibrary;
using MongoDB.Driver;

public class MongoDBConnector
{
    // Field to hold the MongoDB CLient Instance 
    private readonly IMongoDatabase _database;

    // Constructor takes a connection string
    public MongoDBConnector(string connectionString)
    {
        // Create a MongoClient using the connection string
        var client = new MongoClient(connectionString);
    }
}
