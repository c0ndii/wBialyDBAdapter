using Microsoft.Data.SqlClient;
using System.Data;
using wBialyDBAdapter.Database.Relational.Entities;
using wBialyDBAdapter.Repository.Relational;

public class EventRepository : IRelationalRepository<Event>
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

		if (_connection.State != ConnectionState.Open)
			await _connection.OpenAsync();

		var postId = (int)await cmd.ExecuteScalarAsync();

		// Add tags to join table if provided
		if (ev.EventTags != null && ev.EventTags.Count > 0)
		{
			await AddTagsToEventAsync(postId, ev.EventTags);
		}

		return postId;
	}

	public async Task<Event?> GetAsync(int id)
	{
		using var cmd = _connection.CreateCommand();
		cmd.CommandText = @"SELECT * FROM Events WHERE PostId=@id";
		cmd.AddParam("@id", id);

		if (_connection.State != ConnectionState.Open)
			await _connection.OpenAsync();

		using var r = await cmd.ExecuteReaderAsync();
		if (!await r.ReadAsync()) return null;

		var evt = new Event
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

		await r.CloseAsync();
		evt.EventTags = await LoadTagsForEventAsync(evt.PostId);

		return evt;
	}

	public async Task<IEnumerable<Event>> GetAllAsync()
	{
		var list = new List<Event>();

		using var cmd = _connection.CreateCommand();
		cmd.CommandText = @"SELECT * FROM Events";

		if (_connection.State != ConnectionState.Open)
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

		await r.CloseAsync();

		foreach (var evt in list)
		{
			evt.EventTags = await LoadTagsForEventAsync(evt.PostId);
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

		if (_connection.State != ConnectionState.Open)
			await _connection.OpenAsync();

		await cmd.ExecuteNonQueryAsync();

		// Remove existing tags and add new ones
		await RemoveTagsFromEventAsync(ev.PostId);
		if (ev.EventTags != null && ev.EventTags.Count > 0)
		{
			await AddTagsToEventAsync(ev.PostId, ev.EventTags);
		}
	}

	public async Task DeleteAsync(int id)
	{
		using var cmd = _connection.CreateCommand();
		cmd.CommandText = @"DELETE FROM Events WHERE PostId=@id";
		cmd.AddParam("@id", id);

		if (_connection.State != ConnectionState.Open)
			await _connection.OpenAsync();

		await cmd.ExecuteNonQueryAsync();
	}

	private async Task<ICollection<Tag_Event>> LoadTagsForEventAsync(int eventId)
	{
		var tags = new List<Tag_Event>();

		using var cmd = _connection.CreateCommand();
		cmd.CommandText = @"
            SELECT t.TagID, t.Name
            FROM Tag t
            INNER JOIN Event_Tag_Join etj ON t.TagID = etj.TagId
            WHERE etj.EventId = @eventId";
		cmd.AddParam("@eventId", eventId);

		if (_connection.State != ConnectionState.Open)
			await _connection.OpenAsync();

		using var r = await cmd.ExecuteReaderAsync();

		while (await r.ReadAsync())
		{
			tags.Add(new Tag_Event
			{
				TagID = r.GetInt32("TagID"),
				Name = r.GetString("Name")
			});
		}

		return tags;
	}

	private async Task AddTagsToEventAsync(int eventId, ICollection<Tag_Event> tags)
	{
		if (tags == null || tags.Count == 0)
			return;

		foreach (var tag in tags)
		{
			using var cmd = _connection.CreateCommand();
			cmd.CommandText = @"
                INSERT INTO Event_Tag_Join (EventId, TagId)
                SELECT @EventId, TagID FROM Tag WHERE Name = @TagName";

			cmd.AddParam("@EventId", eventId);
			cmd.AddParam("@TagName", tag.Name);

			if (_connection.State != ConnectionState.Open)
				await _connection.OpenAsync();

			await cmd.ExecuteNonQueryAsync();
		}
	}

	private async Task RemoveTagsFromEventAsync(int eventId)
	{
		using var cmd = _connection.CreateCommand();
		cmd.CommandText = @"DELETE FROM Event_Tag_Join WHERE EventId = @eventId";
		cmd.AddParam("@eventId", eventId);

		if (_connection.State != ConnectionState.Open)
			await _connection.OpenAsync();

		await cmd.ExecuteNonQueryAsync();
	}
}
