using Microsoft.Data.SqlClient;
using System.Data;
using wBialyDBAdapter.Database.Relational.Entities;
using wBialyDBAdapter.Repository.Relational;

namespace wBialyDBAdapter.Repository.Relational.Implementation
{
    public class TagRepository : IRelationalRepository<Tag>
    {
        private readonly SqlConnection _connection;

        public TagRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> CreateAsync(Tag t)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Tag (Name)
                OUTPUT INSERTED.TagID
                VALUES (@Name)";

            cmd.AddParam("@Name", t.Name);

            if (_connection.State != ConnectionState.Open)
                await _connection.OpenAsync();

            return (int)await cmd.ExecuteScalarAsync();
        }

        public async Task<Tag?> GetAsync(int id)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Tag WHERE TagID=@id";
            cmd.AddParam("@id", id);

            if (_connection.State != ConnectionState.Open)
                await _connection.OpenAsync();

            using var r = await cmd.ExecuteReaderAsync();
            if (!await r.ReadAsync()) return null;

            return new Tag
            {
                TagID = r.GetInt32("TagID"),
                Name = r.GetString("Name")
            };
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            var list = new List<Tag>();

            using var cmd = _connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Tag ORDER BY TagID";

            if (_connection.State != ConnectionState.Open)
                await _connection.OpenAsync();

            using var r = await cmd.ExecuteReaderAsync();

            while (await r.ReadAsync())
            {
                list.Add(new Tag
                {
                    TagID = r.GetInt32("TagID"),
                    Name = r.GetString("Name")
                });
            }

            return list;
        }

        public async Task UpdateAsync(Tag t)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = @"
                UPDATE Tag SET
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
            cmd.CommandText = @"DELETE FROM Tag WHERE TagID=@id";
            cmd.AddParam("@id", id);

            if (_connection.State != ConnectionState.Open)
                await _connection.OpenAsync();

            await cmd.ExecuteNonQueryAsync();
        }
    }
}
