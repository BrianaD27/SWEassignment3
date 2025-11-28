using DBConnectorLibrary;

class Program
{
    static void Main(string[] args)
    {
        IdbConnector connector = null;
        bool running = true;

        Console.WriteLine("=== Database Connection REPL ===");
        Console.WriteLine("Type 'help' for available commands\n");

        while (running)
        {
            try
            {
                Console.Write("> ");
                string input = Console.ReadLine()?.Trim() ?? "";

                if (string.IsNullOrEmpty(input))
                    continue;

                string[] parts = input.Split(' ', 2);
                string command = parts[0].ToLower();

                switch (command)
                {
                    case "help":
                        DisplayHelp();
                        break;

                    case "connect":
                        connector = HandleConnect(parts);
                        break;

                    case "ping":
                        HandlePing(connector);
                        break;

                    case "status":
                        DisplayStatus(connector);
                        break;

                    case "exit":
                    case "quit":
                        running = false;
                        Console.WriteLine("Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Unknown command. Type 'help' for available commands.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    static void DisplayHelp()
    {
        Console.WriteLine(@"
Available Commands:
  connect <database> <connection-string>
    - Establishes connection to MongoDB or PostgreSQL
    - Example: connect mongodb mongodb://localhost:27017
    - Example: connect postgresql Server=localhost;Port=5432;Database=mydb;

  ping
    - Tests the database connection
    - Returns success or failure status

  status
    - Shows current connection status

  help
    - Displays this help menu

  exit / quit
    - Closes the application
");
    }

    static IdbConnector HandleConnect(string[] parts)
    {
        if (parts.Length < 2)
        {
            Console.WriteLine("Usage: connect <database> <connection-string>");
            Console.WriteLine("Supported databases: mongodb, postgresql");
            return null;
        }

        string[] connParts = parts[1].Split(' ', 2);
        string database = connParts[0].ToLower();
        string connectionString = connParts.Length > 1 ? connParts[1] : "";

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            Console.WriteLine("Please provide a connection string");
            return null;
        }

        IdbConnector connector = database switch
        {
            "mongodb" => new MongoDBConnector(connectionString),
            "postgresql" => new PostgresConnector(connectionString),
            _ => throw new ArgumentException($"Unsupported database: {database}")
        };

        Console.WriteLine($"✓ Connector created for {database}");
        return connector;
    }

    static void HandlePing(IdbConnector connector)
    {
        if (connector == null)
        {
            Console.WriteLine("✗ No database connection established. Use 'connect' first.");
            return;
        }

        try
        {
            bool success = connector.Ping();
            if (success)
            {
                Console.WriteLine("✓ Ping successful! Database is reachable.");
            }
            else
            {
                Console.WriteLine("✗ Ping failed! Could not reach the database.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Ping error: {ex.Message}");
        }
    }

    static void DisplayStatus(IdbConnector connector)
    {
        if (connector == null)
        {
            Console.WriteLine("Status: No connection established");
        }
        else
        {
            string dbType = connector.GetType().Name;
            Console.WriteLine($"Status: Connected via {dbType}");
        }
    }
}
