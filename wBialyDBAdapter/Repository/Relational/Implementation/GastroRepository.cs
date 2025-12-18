using Microsoft.Data.SqlClient;
using System.Data;
using wBialyDBAdapter.Database.Relational.Entities;
using wBialyDBAdapter.Repository.Relational;

public class GastroRepository : IRelationalRepository<Gastro>
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

        if (_connection.State != ConnectionState.Open)
            await _connection.OpenAsync();

        var postId = (int)await cmd.ExecuteScalarAsync();

        // Add tags to join table if provided
        if (g.GastroTags != null && g.GastroTags.Count > 0)
        {
            await AddTagsToGastroAsync(postId, g.GastroTags);
        }

        return postId;
    }

    public async Task<Gastro?> GetAsync(int id)
    {
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = @"SELECT * FROM Gastro WHERE PostId=@id";
        cmd.AddParam("@id", id);

        if (_connection.State != ConnectionState.Open)
            await _connection.OpenAsync();

        using var r = await cmd.ExecuteReaderAsync();
        if (!await r.ReadAsync()) return null;

        var gastro = new Gastro
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

        await r.CloseAsync();
        gastro.GastroTags = await LoadTagsForGastroAsync(gastro.PostId);

        return gastro;
    }

    public async Task<IEnumerable<Gastro>> GetAllAsync()
    {
        var list = new List<Gastro>();

        using var cmd = _connection.CreateCommand();
        cmd.CommandText = @"SELECT * FROM Gastro";

        if (_connection.State != ConnectionState.Open)
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

        await r.CloseAsync();

        foreach (var gastro in list)
        {
            gastro.GastroTags = await LoadTagsForGastroAsync(gastro.PostId);
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

        if (_connection.State != ConnectionState.Open)
            await _connection.OpenAsync();

        await cmd.ExecuteNonQueryAsync();

        // Remove existing tags and add new ones
        await RemoveTagsFromGastroAsync(g.PostId);
        if (g.GastroTags != null && g.GastroTags.Count > 0)
        {
            await AddTagsToGastroAsync(g.PostId, g.GastroTags);
        }
    }

    public async Task DeleteAsync(int id)
    {
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = @"DELETE FROM Gastro WHERE PostId=@id";
        cmd.AddParam("@id", id);

        if (_connection.State != ConnectionState.Open)
            await _connection.OpenAsync();

        await cmd.ExecuteNonQueryAsync();
    }

    private async Task<ICollection<Tag_Gastro>> LoadTagsForGastroAsync(int gastroId)
    {
        var tags = new List<Tag_Gastro>();

        using var cmd = _connection.CreateCommand();
        cmd.CommandText = @"
            SELECT t.TagID, t.Name
            FROM Tag t
            INNER JOIN Gastro_Tag_Join gtj ON t.TagID = gtj.TagId
            WHERE gtj.GastroId = @gastroId";
        cmd.AddParam("@gastroId", gastroId);

        if (_connection.State != ConnectionState.Open)
            await _connection.OpenAsync();

        using var r = await cmd.ExecuteReaderAsync();

        while (await r.ReadAsync())
        {
            tags.Add(new Tag_Gastro
            {
                TagID = r.GetInt32("TagID"),
                Name = r.GetString("Name")
            });
        }

        return tags;
    }

    private async Task AddTagsToGastroAsync(int gastroId, ICollection<Tag_Gastro> tags)
    {
        if (tags == null || tags.Count == 0)
            return;

        foreach (var tag in tags)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Gastro_Tag_Join (GastroId, TagId)
                SELECT @GastroId, TagID FROM Tag WHERE Name = @TagName";

            cmd.AddParam("@GastroId", gastroId);
            cmd.AddParam("@TagName", tag.Name);

            if (_connection.State != ConnectionState.Open)
                await _connection.OpenAsync();

            await cmd.ExecuteNonQueryAsync();
        }
    }

    private async Task RemoveTagsFromGastroAsync(int gastroId)
    {
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = @"DELETE FROM Gastro_Tag_Join WHERE GastroId = @gastroId";
        cmd.AddParam("@gastroId", gastroId);

        if (_connection.State != ConnectionState.Open)
            await _connection.OpenAsync();

        await cmd.ExecuteNonQueryAsync();
    }
}
