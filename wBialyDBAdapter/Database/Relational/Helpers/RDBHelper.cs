using Microsoft.Data.SqlClient;

namespace wBialyDBAdapter.Database.Relational.Helpers
{
    public static class RDBHelper
    {
        public static async Task EnsureRelationalDatabaseInitializedAsync(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            var dbName = builder.InitialCatalog;

            builder.InitialCatalog = "master";
            using (var masterConn = new SqlConnection(builder.ConnectionString))
            {
                await masterConn.OpenAsync();

                using var cmd = masterConn.CreateCommand();
                cmd.CommandText = $@"
                    IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{dbName}')
                    BEGIN
                        CREATE DATABASE [{dbName}];
                    END";
                await cmd.ExecuteNonQueryAsync();
            }

            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                var scriptPath = Path.Combine(AppContext.BaseDirectory, "Resources", "MyRDB.sql");
                var script = await File.ReadAllTextAsync(scriptPath);

                var batches = script.Split(
                    new[] { "GO", "go", "Go" },
                    StringSplitOptions.RemoveEmptyEntries);

                foreach (var batch in batches)
                {
                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = batch;
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
