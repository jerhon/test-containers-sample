using System.Data;
using System.Reflection;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using Npgsql;

// Important: Init scripts are copied to the bin directory when compiled.
var initScriptsPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
initScriptsPath = Path.Combine(initScriptsPath, "DbScripts");

var password = Guid.NewGuid().ToString();
var container = new ContainerBuilder()
    .WithImage("postgres:16")
    .WithPortBinding(5432, true)
    .WithBindMount(initScriptsPath, "/docker-entrypoint-initdb.d", AccessMode.ReadOnly)
    .WithEnvironment("POSTGRES_PASSWORD", password)
    .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
    .Build();

try {
    await container.StartAsync();
}
finally {
 
    var (stdout, stderr) = await container.GetLogsAsync();

    Console.WriteLine(stdout);   
}

var connectionString = $"Host=localhost;Port={container.GetMappedPublicPort(5432)};Username=postgres;Password={password};Database=starfleet";
using NpgsqlConnection connection = new NpgsqlConnection(connectionString);
await connection.OpenAsync();

using var crewMembersCommand = connection.CreateCommand();
crewMembersCommand.CommandText = "SELECT * FROM CrewMembers";

using var crewMembersReader = await crewMembersCommand.ExecuteReaderAsync();

while (await crewMembersReader.ReadAsync()) {
    string name = crewMembersReader.GetFieldValue<string>("Name");
    Console.WriteLine(name);
}

