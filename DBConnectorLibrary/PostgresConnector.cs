namespace DBConnectorLibrary;
using Npgsql;

public class PostgresConnector : IdbConnector
{
    // Private field to store the connection string
    private readonly string _connectionString;

    // Constructor
    public PostgresConnector(string connectionString)
    {
        _connectionString = connectionString;
    }

    // Implementation of Ping Method
    public bool Ping()
    {
        try
        {
            // Tries to open connection to Postgres
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            // If connection is Open them immediately close it and Return True
            connection.Close();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
