namespace TestDBConnector;

using System.Threading.Tasks;
using DBConnectorLibrary;
using Testcontainers.PostgreSql;
using Xunit;

public class PostgresTests : IAsyncLifetime
{
    private string _connectionString = string.Empty;

    private readonly PostgreSqlContainer _pg = new PostgreSqlBuilder()
        .WithImage("postgres:16-alpine")
        .Build();

    public async Task InitializeAsync()
    {
        await _pg.StartAsync();
        _connectionString = _pg.GetConnectionString();
    }

    public async Task DisposeAsync()
    {
        await _pg.DisposeAsync();
    }
    
    [Fact]
    public void Postgres_Ping_Returns_True()
    {
        var connector = new PostgresConnector(_connectionString);
        Assert.True(connector.Ping());
    }

    [Fact]
    public void Postgres_Ping_Returns_False()
    {
        var badConnection = "postgres://localhost:29999";
        var connector = new PostgresConnector(badConnection);

        Assert.False(connector.Ping());
    }

}