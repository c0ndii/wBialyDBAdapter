using wBialyBezdomnyEdition.Config;
using wBialyBezdomnyEdition.Database.NoSQL;
using wBialyDBAdapter.Config;
using wBialyBezdomnyEdition.Repository.NoSQL;
using wBialyDBAdapter.Services;
using wBialyBezdomnyEdition.Database.NoSQL.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Section with all databases configuration
var databaseSections = builder.Configuration.GetSection("Databases");

// Add Relational Database
builder.Services.Configure<RelationalDBSettings>(databaseSections.GetSection("RelationalDatabaseSettings"));

// Add Object Relational Database
builder.Services.Configure<ObjectRelationalDBSettings>(databaseSections.GetSection("ObjectRelationalDatabaseSettings"));

// Add NoSQL Database
builder.Services.Configure<NoSQLDBSettings>(databaseSections.GetSection("NoSQLDatabaseSettings"));

builder.Services.AddSingleton<NoSQLDB>();

builder.Services.AddScoped<IOnSiteRepository, OnSiteRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IGastroRepository, GastroRepository>();

builder.Services.AddScoped<IBaseRepository<OnSite>>(sp =>
    sp.GetRequiredService<IOnSiteRepository>());

builder.Services.AddScoped<IBaseRepository<Event>>(sp =>
    sp.GetRequiredService<IEventRepository>());

builder.Services.AddScoped<IBaseRepository<Gastro>>(sp =>
    sp.GetRequiredService<IGastroRepository>());

builder.Services.AddScoped<IQueryService<Event>, EventService>();
builder.Services.AddScoped<IQueryService<Gastro>, GastroService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
