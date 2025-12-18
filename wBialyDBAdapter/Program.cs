using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using wBialyDBAdapter.Config;
using wBialyDBAdapter.Database.NoSQL;
using wBialyDBAdapter.Database.NoSQL.Entities;
using wBialyDBAdapter.Database.ObjectRelational;
using wBialyDBAdapter.Database.Relational.Helpers;
using wBialyDBAdapter.Mapping;
using wBialyDBAdapter.Mapping.Implementation;
using wBialyDBAdapter.Model;
using wBialyDBAdapter.Repository.NoSQL;
using wBialyDBAdapter.Repository.ObjectRelational;
using wBialyDBAdapter.Repository.Relational;
using wBialyDBAdapter.Services;
using wBialyDBAdapter.Services.Implementation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS: allow frontend origin
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        // Allow localhost:3000 by default; can be overridden via env CORS__AllowedOrigins
        var allowedOrigins = builder.Configuration.GetSection("CORS:AllowedOrigins").Get<string[]>()
            ?? new[] { "http://localhost:3000" };
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Section with all databases configuration
var databaseSections = builder.Configuration.GetSection("Databases");

// Add Relational Database
builder.Services.Configure<RelationalDBSettings>(databaseSections.GetSection("RelationalDatabaseSettings"));
builder.Services.AddScoped<SqlConnection>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<RelationalDBSettings>>().Value;
    return new SqlConnection(settings.ConnectionString);
});

// Add Object Relational Database
builder.Services.Configure<ObjectRelationalDBSettings>(databaseSections.GetSection("ObjectRelationalDatabaseSettings"));
builder.Services.AddDbContext<ORDB>((serviceProvider, options) =>
{
    options.UseSqlServer(serviceProvider
        .GetRequiredService<IOptions<ObjectRelationalDBSettings>>()
        .Value.ConnectionString);
});

// Add NoSQL Database
builder.Services.Configure<NoSQLDBSettings>(databaseSections.GetSection("NoSQLDatabaseSettings"));
builder.Services.AddSingleton<NoSQLDB>();

// Add mappers
builder.Services.AddScoped<IEventMapper, EventMapper>();
builder.Services.AddScoped<IGastroMapper, GastroMapper>();

builder.Services.AddScoped<ITagMapper, TagMapper>();
builder.Services.AddScoped<ITagService, TagService>();

// Add Relational Repositories
builder.Services.AddScoped<IRelationalRepository<wBialyDBAdapter.Database.Relational.Entities.Event>, EventRepository>();
builder.Services.AddScoped<IRelationalRepository<wBialyDBAdapter.Database.Relational.Entities.Gastro>, GastroRepository>();
builder.Services.AddScoped<IRelationalRepository<wBialyDBAdapter.Database.Relational.Entities.Tag>, wBialyDBAdapter.Repository.Relational.Implementation.TagRepository>();
builder.Services.AddScoped<IRelationalRepository<wBialyDBAdapter.Database.Relational.Entities.Tag_Event>, wBialyDBAdapter.Repository.Relational.Implementation.TagEventRepository>();
builder.Services.AddScoped<IRelationalRepository<wBialyDBAdapter.Database.Relational.Entities.Tag_Gastro>, wBialyDBAdapter.Repository.Relational.Implementation.TagGastroRepository>();

// Add Object Relational Repositories
builder.Services.AddScoped<IObjectRelationalRepository<wBialyDBAdapter.Database.ObjectRelational.Entities.Event>, wBialyDBAdapter.Repository.ObjectRelational.Implementation.EventRepository>();
builder.Services.AddScoped<IObjectRelationalRepository<wBialyDBAdapter.Database.ObjectRelational.Entities.Gastro>, wBialyDBAdapter.Repository.ObjectRelational.Implementation.GastroRepository>();
builder.Services.AddScoped<IObjectRelationalRepository<wBialyDBAdapter.Database.ObjectRelational.Entities.Tag_Event>, wBialyDBAdapter.Repository.ObjectRelational.Implementation.TagEventRepository>();
builder.Services.AddScoped<IObjectRelationalRepository<wBialyDBAdapter.Database.ObjectRelational.Entities.Tag_Gastro>, wBialyDBAdapter.Repository.ObjectRelational.Implementation.TagGastroRepository>();

// Add NoSQL Repositories
builder.Services.AddScoped<IBaseRepository<Event>, wBialyDBAdapter.Repository.NoSQL.Implementation.EventRepository>();
builder.Services.AddScoped<IBaseRepository<Gastro>, wBialyDBAdapter.Repository.NoSQL.Implementation.GastroRepository>();
builder.Services.AddScoped<IBaseRepository<Tag>, wBialyDBAdapter.Repository.NoSQL.Implementation.TagRepository>();

// Add services
builder.Services.AddScoped<IQueryService<UnifiedEventModel>, EventService>();
builder.Services.AddScoped<IQueryService<UnifiedGastroModel>, GastroService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ORDB>();
    if (db != null)
    {
        db.Database.Migrate();
    }
}

using (var scope = app.Services.CreateScope())
{
    var conn = scope.ServiceProvider.GetRequiredService<IOptions<RelationalDBSettings>>();
    if (conn != null)
    {
        await RDBHelper.EnsureRelationalDatabaseInitializedAsync(conn.Value.ConnectionString);
    }
}

using (var scope = app.Services.CreateScope())
{
    var noSqlDb = scope.ServiceProvider.GetRequiredService<NoSQLDB>();
    if (noSqlDb != null)
    {
        await wBialyDBAdapter.Database.NoSQL.NoSQLSeeder.SeedAsync(noSqlDb);
    }
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();
