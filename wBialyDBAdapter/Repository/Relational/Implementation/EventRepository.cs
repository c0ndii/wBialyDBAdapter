using Microsoft.Data.SqlClient;
using System.Data;
using wBialyBezdomnyEdition.Database.Relational.Entities;
using wBialyDBAdapter.Repository.Relational;

public class EventRepository : IEventRepository
{
    private readonly SqlConnection _connection;

    public EventRepository(SqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<int> CreateAsync(Event ev)
    {
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO Events (Title, Description, AddDate, Place, Author, Link, EventDate)
            OUTPUT INSERTED.PostId
            VALUES (@Title, @Desc, @AddDate, @Place, @Author, @Link, @EventDate)";

        cmd.AddParam("@Title", ev.Title);
        cmd.AddParam("@Desc", ev.Description);
        cmd.AddParam("@AddDate", ev.AddDate);
        cmd.AddParam("@Place", ev.Place);
        cmd.AddParam("@Author", ev.Author);
        cmd.AddParam("@Link", ev.Link);
        cmd.AddParam("@EventDate", ev.EventDate);

        if (_connection.State != System.Data.ConnectionState.Open)
            await _connection.OpenAsync();

        return (int)await cmd.ExecuteScalarAsync();
    }

    public async Task<Event?> GetAsync(int id)
    {
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = @"SELECT * FROM Events WHERE PostId=@id";
        cmd.AddParam("@id", id);

        if (_connection.State != System.Data.ConnectionState.Open)
            await _connection.OpenAsync();

        using var r = await cmd.ExecuteReaderAsync();
        if (!await r.ReadAsync()) return null;

        return new Event
        {
            PostId = r.GetInt32("PostId"),
            Title = r.GetString("Title"),
            Description = r.GetString("Description"),
            AddDate = r.GetDateTime("AddDate"),
            Place = r.GetString("Place"),
            Author = r.GetString("Author"),
            Link = r.GetString("Link"),
            EventDate = r.GetDateTime("EventDate"),
        };
    }

    public async Task<IEnumerable<Event>> GetAllAsync()
    {
        var list = new List<Event>();

        using var cmd = _connection.CreateCommand();
        cmd.CommandText = @"SELECT * FROM Events";

        if (_connection.State != System.Data.ConnectionState.Open)
            await _connection.OpenAsync();

        using var r = await cmd.ExecuteReaderAsync();

        while (await r.ReadAsync())
        {
            list.Add(new Event
            {
                PostId = r.GetInt32("PostId"),
                Title = r.GetString("Title"),
                Description = r.GetString("Description"),
                AddDate = r.GetDateTime("AddDate"),
                Place = r.GetString("Place"),
                Author = r.GetString("Author"),
                Link = r.GetString("Link"),
                EventDate = r.GetDateTime("EventDate"),
            });
        }

        return list;
    }

    public async Task UpdateAsync(Event ev)
    {
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = @"
            UPDATE Events SET
                Title=@Title,
                Description=@Desc,
                AddDate=@AddDate,
                Place=@Place,
                Author=@Author,
                Link=@Link,
                EventDate=@EventDate
            WHERE PostId=@id";

        cmd.AddParam("@Title", ev.Title);
        cmd.AddParam("@Desc", ev.Description);
        cmd.AddParam("@AddDate", ev.AddDate);
        cmd.AddParam("@Place", ev.Place);
        cmd.AddParam("@Author", ev.Author);
        cmd.AddParam("@Link", ev.Link);
        cmd.AddParam("@EventDate", ev.EventDate);
        cmd.AddParam("@id", ev.PostId);

        if (_connection.State != System.Data.ConnectionState.Open)
            await _connection.OpenAsync();

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(int id)
    {
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = @"DELETE FROM Events WHERE PostId=@id";
        cmd.AddParam("@id", id);

        if (_connection.State != System.Data.ConnectionState.Open)
            await _connection.OpenAsync();

        await cmd.ExecuteNonQueryAsync();
    }
}
