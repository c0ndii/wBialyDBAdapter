using Microsoft.Data.SqlClient;
using System.Data;
using wBialyDBAdapter.Database.Relational.Entities;
using wBialyDBAdapter.Repository.Relational;

namespace wBialyDBAdapter.Repository.Relational.Implementation
{
    public class TagGastroRepository : IRelationalRepository<Tag_Gastro>
    {
        private readonly SqlConnection _connection;

        public TagGastroRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> CreateAsync(Tag_Gastro t)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Tag_Gastro (Name)
                OUTPUT INSERTED.TagID
                VALUES (@Name)";

            cmd.AddParam("@Name", t.Name);

            if (_connection.State != ConnectionState.Open)
                await _connection.OpenAsync();

            return (int)await cmd.ExecuteScalarAsync();
        }

        public async Task<Tag_Gastro?> GetAsync(int id)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Tag_Gastro WHERE TagID=@id";
            cmd.AddParam("@id", id);

            if (_connection.State != ConnectionState.Open)
                await _connection.OpenAsync();

            using var r = await cmd.ExecuteReaderAsync();
            if (!await r.ReadAsync()) return null;

            return new Tag_Gastro
            {
                TagID = r.GetInt32("TagID"),
                Name = r.GetString("Name")
            };
        }

        public async Task<IEnumerable<Tag_Gastro>> GetAllAsync()
        {
            var list = new List<Tag_Gastro>();

            using var cmd = _connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Tag_Gastro";

            if (_connection.State != ConnectionState.Open)
                await _connection.OpenAsync();

            using var r = await cmd.ExecuteReaderAsync();

            while (await r.ReadAsync())
            {
                list.Add(new Tag_Gastro
                {
                    TagID = r.GetInt32("TagID"),
                    Name = r.GetString("Name")
                });
            }

            return list;
        }

        public async Task UpdateAsync(Tag_Gastro t)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = @"
                UPDATE Tag_Gastro SET
                    Name=@Name
                WHERE TagID=@id";

            cmd.AddParam("@Name", t.Name);
            cmd.AddParam("@id", t.TagID);

            if (_connection.State != ConnectionState.Open)
                await _connection.OpenAsync();

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM Tag_Gastro WHERE TagID=@id";
            cmd.AddParam("@id", id);

            if (_connection.State != ConnectionState.Open)
                await _connection.OpenAsync();

            await cmd.ExecuteNonQueryAsync();
        }
    }
}