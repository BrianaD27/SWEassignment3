namespace TestDBConnector;

using System.Threading.Tasks;
using DBConnectorLibrary; 
using Testcontainers.MongoDb;
using Xunit;

public class UnitTest1 : IAsyncLifetime
{
    private string _connectionString = string.Empty;
    private readonly MongoDbContainer _mongo = new MongoDbBuilder().WithImage("mongo:7").Build();

    public async Task InitializeAsync()
    {
        await _mongo.StartAsync();
        _connectionString = _mongo.GetConnectionString();
    }

    public async Task DisposeAsync()
    {
        await _mongo.DisposeAsync();
    }

    [Fact]
    public void MongoDB_Ping_Returns_True()
    {
        var connector = new MongoDBConnector(_connectionString);
        Assert.True(connector.Ping());
    }

    [Fact]
    public void MongoDB_Ping_Returns_False()
    {
        var badConnection = "mongodb://localhost:29999";
        var connector = new MongoDBConnector(badConnection);

        Assert.False(connector.Ping());
    }

}
