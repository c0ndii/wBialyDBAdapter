using Microsoft.Data.SqlClient;
using System.Data;
using wBialyBezdomnyEdition.Database.Relational.Entities;
using wBialyDBAdapter.Repository.Relational;

public class GastroRepository : IGastroRepository
{
    private readonly SqlConnection _connection;

    public GastroRepository(SqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<int> CreateAsync(Gastro g)
    {
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO Gastro (Title, Description, AddDate, Place, Author, Link, Day)
            OUTPUT INSERTED.PostId
            VALUES (@Title, @Desc, @AddDate, @Place, @Author, @Link, @Day)";

        cmd.AddParam("@Title", g.Title);
        cmd.AddParam("@Desc", g.Description);
        cmd.AddParam("@AddDate", g.AddDate);
        cmd.AddParam("@Place", g.Place);
        cmd.AddParam("@Author", g.Author);
        cmd.AddParam("@Link", g.Link);
        cmd.AddParam("@Day", g.Day);

        if (_connection.State != System.Data.ConnectionState.Open)
            await _connection.OpenAsync();

        return (int)await cmd.ExecuteScalarAsync();
    }

    public async Task<Gastro?> GetAsync(int id)
    {
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = @"SELECT * FROM Gastro WHERE PostId=@id";
        cmd.AddParam("@id", id);

        if (_connection.State != System.Data.ConnectionState.Open)
            await _connection.OpenAsync();

        using var r = await cmd.ExecuteReaderAsync();
        if (!await r.ReadAsync()) return null;

        return new Gastro
        {
            PostId = r.GetInt32("PostId"),
            Title = r.GetString("Title"),
            Description = r.GetString("Description"),
            AddDate = r.GetDateTime("AddDate"),
            Place = r.GetString("Place"),
            Author = r.GetString("Author"),
            Link = r.GetString("Link"),
            Day = r.GetDateTime("Day"),
        };
    }

    public async Task<IEnumerable<Gastro>> GetAllAsync()
    {
        var list = new List<Gastro>();

        using var cmd = _connection.CreateCommand();
        cmd.CommandText = @"SELECT * FROM Gastro";

        if (_connection.State != System.Data.ConnectionState.Open)
            await _connection.OpenAsync();

        using var r = await cmd.ExecuteReaderAsync();

        while (await r.ReadAsync())
        {
            list.Add(new Gastro
            {
                PostId = r.GetInt32("PostId"),
                Title = r.GetString("Title"),
                Description = r.GetString("Description"),
                AddDate = r.GetDateTime("AddDate"),
                Place = r.GetString("Place"),
                Author = r.GetString("Author"),
                Link = r.GetString("Link"),
                Day = r.GetDateTime("Day"),
            });
        }

        return list;
    }

    public async Task UpdateAsync(Gastro g)
    {
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = @"
            UPDATE Gastro SET
                Title=@Title,
                Description=@Desc,
                AddDate=@AddDate,
                Place=@Place,
                Author=@Author,
                Link=@Link,
                Day=@Day
            WHERE PostId=@id";

        cmd.AddParam("@Title", g.Title);
        cmd.AddParam("@Desc", g.Description);
        cmd.AddParam("@AddDate", g.AddDate);
        cmd.AddParam("@Place", g.Place);
        cmd.AddParam("@Author", g.Author);
        cmd.AddParam("@Link", g.Link);
        cmd.AddParam("@Day", g.Day);
        cmd.AddParam("@id", g.PostId);

        if (_connection.State != System.Data.ConnectionState.Open)
            await _connection.OpenAsync();

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(int id)
    {
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = @"DELETE FROM Gastro WHERE PostId=@id";
        cmd.AddParam("@id", id);

        if (_connection.State != System.Data.ConnectionState.Open)
            await _connection.OpenAsync();

        await cmd.ExecuteNonQueryAsync();
    }
}
