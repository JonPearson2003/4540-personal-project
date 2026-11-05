using BuildTrack.Api.Data;
using BuildTrack.Api.Endpoints;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("BuildTrackDatabase")
	?? throw new InvalidOperationException("BuildTrackDatabase is not configured.");

builder.Services.AddValidation();
builder.Services.AddProblemDetails();
builder.Services.AddDbContext<BuildTrackDbContext>(options =>
	options.UseSqlite(connectionString));

builder.Services.AddCors();

var app = builder.Build();
app.UseExceptionHandler();

app.UseCors(builder => builder
	.AllowAnyOrigin()
	.AllowAnyMethod()
	.AllowAnyHeader());

app.MapBuildTrackEndpoints();

await using (var scope = app.Services.CreateAsyncScope())
{
	var database = scope.ServiceProvider.GetRequiredService<BuildTrackDbContext>();
	await database.Database.MigrateAsync();
}

await app.RunAsync();
