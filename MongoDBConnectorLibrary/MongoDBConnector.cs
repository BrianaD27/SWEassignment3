namespace MongoDBConnectorLibrary;
using MongoDB.Bson;
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

        // Default to "admin" database 
        var url = new MongoUrl(connectionString);
        var dbName = string.IsNullOrWhiteSpace(url.DatabaseName) ? "admin" : url.DatabaseName;

        _database = client.GetDatabase(dbName);
    }

    public bool MongoDBPing()
    {
        try
        {
            // Sends the "ping" command to MongoDB
            // The same as running { "ping": 1 } in the database
            var result = _database.RunCommand<BsonDocument>(
                new BsonDocument("ping", 1)
            );

            // MongoDB sends { "ok": 1 } if its running
            return result["ok"] == 1;
        }
        catch
        {
            return false;
        }
    }
}
