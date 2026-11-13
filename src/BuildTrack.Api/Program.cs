using BuildTrack.Api.Data;
using BuildTrack.Api.Entities;
using BuildTrack.Api.Endpoints;
using BuildTrack.Api.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("BuildTrackDatabase")
	?? throw new InvalidOperationException("BuildTrackDatabase is not configured.");

builder.Services.AddValidation();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddScoped<IPasswordHasher<Users>, PasswordHasher<Users>>();
builder.Services.AddDbContext<BuildTrackDbContext>(options =>
	options.UseSqlite(connectionString));

builder.Services.AddCors();

var app = builder.Build();
app.UseExceptionHandler();
if (app.Environment.IsDevelopment())
	app.MapOpenApi();

app.UseCors(builder => builder
	.AllowAnyOrigin()
	.AllowAnyMethod()
	.AllowAnyHeader());

app.MapBuildTrackEndpoints();

await using (var scope = app.Services.CreateAsyncScope())
{
	var database = scope.ServiceProvider.GetRequiredService<BuildTrackDbContext>();
	await database.Database.MigrateAsync();
	var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<Users>>();
	var users = await database.Users.ToListAsync();
	var migratedLegacyPasswords = false;
	foreach (var user in users)
	{
		if (UserPasswordHashing.IsHashed(user.Password))
			continue;

		user.Password = UserPasswordHashing.Hash(user, user.Password, passwordHasher);
		migratedLegacyPasswords = true;
	}

	if (migratedLegacyPasswords)
		await database.SaveChangesAsync();
}

await app.RunAsync();
