using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;
using wBialyBezdomnyEdition.Config;
using wBialyBezdomnyEdition.Database.NoSQL;
using wBialyBezdomnyEdition.Database.NoSQL.Entities;
using wBialyBezdomnyEdition.Database.ObjectRelational;
using wBialyBezdomnyEdition.Repository.NoSQL;
using wBialyDBAdapter.Config;
using wBialyDBAdapter.Repository.NoSQL.Implementation;
using wBialyDBAdapter.Repository.ObjectRelational;
using wBialyDBAdapter.Repository.ObjectRelational.Implementation;
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

// Add Relational Repositories
builder.Services.AddScoped<wBialyDBAdapter.Repository.Relational.IEventRepository, EventRepository>();
builder.Services.AddScoped<wBialyDBAdapter.Repository.Relational.IGastroRepository, GastroRepository>();

// Add Object Relational Repositories
builder.Services.AddScoped<wBialyDBAdapter.Repository.ObjectRelational.IEventRepository, wBialyDBAdapter.Repository.ObjectRelational.Implementation.EventRepository>();
builder.Services.AddScoped<wBialyDBAdapter.Repository.ObjectRelational.IGastroRepository, wBialyDBAdapter.Repository.ObjectRelational.Implementation.GastroRepository>();

// Add NoSQL Repositories
builder.Services.AddScoped<IBaseRepository<Event>, wBialyDBAdapter.Repository.NoSQL.Implementation.EventRepository>();
builder.Services.AddScoped<IBaseRepository<Gastro>, wBialyDBAdapter.Repository.NoSQL.Implementation. GastroRepository>();
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
