using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using StaffingApi.Repositories.EF;
using StaffingApi.Repositories.EF.Config;
using StaffingApi.Repositories.EF.Context;
using StaffingApi.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddRouting(opt => opt.LowercaseUrls = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});
builder.Services.AddCors();

var mongoClient = new MongoClient(configuration["ConnectionString"]);
builder.Services.AddSingleton<IMongoClient>(mongoClient);
if (configuration["DatabaseName"] == null)
{
    throw new ArgumentNullException("DatabaseName", "DatabaseName is not set in the configuration.");
}

// Register MongoDB configuration
builder.Services.AddSingleton<IMongoConfig>(s => new MongoConfig(mongoClient, configuration["DatabaseName"]));

// Register DbContext and repositories
builder.Services.AddDbContext<LineUpDbContext>((sp, options) =>
{
    var mongoConfig = sp.GetRequiredService<IMongoConfig>();
    options.UseMongoDB(mongoConfig.MongoClient, mongoConfig.DatabaseName);
});
builder.Services.AddScoped<IPlayerContextRepository, PlayerContextRepository>();

builder.Services.AddDbContext<PlayerDbContext>((sp, options) =>
{
    var mongoConfig = sp.GetRequiredService<IMongoConfig>();
    options.UseMongoDB(mongoConfig.MongoClient, mongoConfig.DatabaseName);
});
builder.Services.AddScoped<ILineUpContextRepository, LineUpContextRepository>();

// Register services
builder.Services.AddTransient<IPlayerService, PlayerService>();
builder.Services.AddTransient<ILineUpService, LineUpService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(b =>
        b.WithOrigins(["http://localhost:4200", "http://localhost:5173", "http://localhost:3000"])
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()  // Add AllowCredentials for SignalR to work with cookies or authorization headers
);
app.UseRouting();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI(opt =>
{
    opt.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});
app.MapControllers();
app.Run();