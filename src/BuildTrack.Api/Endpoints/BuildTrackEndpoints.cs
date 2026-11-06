using BuildTrack.Api.Data;
using BuildTrack.Api.Entities;
using BuildTrack.Api.Security;
using BuildTrack.Contracts.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BuildTrack.Api.Endpoints;

public static class BuildTrackEndpoints
{
	public static WebApplication MapBuildTrackEndpoints(this WebApplication app)
	{
		var group = app.MapGroup("/api/buildtrack");
		group.MapGet("/users", GetAllUsersAsync);
		group.MapGet("/users/{id:int}", GetByUserIdAsync).WithName(nameof(GetByUserIdAsync));
		group.MapPost("/users", CreateUserAsync);
		group.MapPut("/users/{id:int}", UpdateUserAsync);
		group.MapDelete("/users/{id:int}", DeleteUserAsync);
		return app;
	}

	private static async Task<Ok<List<UsersDto>>> GetAllUsersAsync(
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var items = await database.Users.AsNoTracking()
			.OrderBy(item => item.Name)
			.Select(item => new UsersDto(
				item.UserId, item.Name, item.Email, item.Role))
			.ToListAsync(cancellationToken);
		return TypedResults.Ok(items);
	}

	private static async Task<Results<Ok<UsersDto>, NotFound>> GetByUserIdAsync(
		int id,
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var item = await database.Users.AsNoTracking()
			.Where(item => item.UserId == id)
			.Select(item => new UsersDto(
				item.UserId, item.Name, item.Email, item.Role))
			.SingleOrDefaultAsync(cancellationToken);
		return item is null ? TypedResults.NotFound() : TypedResults.Ok(item);
	}

	// private static async Task<Results<Ok<AssignmentDto>, NotFound>> GetByIdAsync(
	// 	int id,
	// 	AssignmentsDbContext database,
	// 	CancellationToken cancellationToken)
	// {
	// 	var item = await database.Assignments.AsNoTracking()
	// 		.Where(item => item.AssignmentId == id)
	// 		.Select(item => new AssignmentDto(
	// 			item.AssignmentId, item.Title, item.DueDate, item.IsCompleted))
	// 		.SingleOrDefaultAsync(cancellationToken);
	// 	return item is null ? TypedResults.NotFound() : TypedResults.Ok(item);
	// }

	private static async Task<Results<Created<UsersDto>, ValidationProblem>> CreateUserAsync(
		CreateUserRequest request,
		BuildTrackDbContext database,
		IPasswordHasher<Users> passwordHasher,
		CancellationToken cancellationToken)
	{
		var validationProblem = ValidateUserEmail(request.Email);
		if (validationProblem is not null)
			return validationProblem;

		var entity = new Users
		{
			Name = request.Name.Trim(),
			Email = request.Email.Trim(),
			Password = string.Empty,
			Role = request.Role.Trim()
		};
		entity.Password = UserPasswordHashing.Hash(entity, request.Password, passwordHasher);
		database.Users.Add(entity);
		await database.SaveChangesAsync(cancellationToken);

		var result = ToDto(entity);
		return TypedResults.Created($"/api/users/{entity.UserId}", result);
	}

	private static async Task<Results<NoContent, NotFound, ValidationProblem>> UpdateUserAsync(
		int id,
		UpdateUserRequest request,
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var validationProblem = ValidateUserEmail(request.Email);
		if (validationProblem is not null)
			return validationProblem;

		var entity = await database.Users.FindAsync([id], cancellationToken);
		if (entity is null)
			return TypedResults.NotFound();

		entity.Name = request.Name.Trim();
		entity.Email = request.Email.Trim();
		entity.Role = request.Role.Trim();
		await database.SaveChangesAsync(cancellationToken);
		return TypedResults.NoContent();
	}

	private static async Task<Results<NoContent, NotFound>> DeleteUserAsync(
		int id,
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var entity = await database.Users.FindAsync([id], cancellationToken);
		if (entity is null)
			return TypedResults.NotFound();

		database.Users.Remove(entity);
		await database.SaveChangesAsync(cancellationToken);
		return TypedResults.NoContent();
	}

	private static ValidationProblem? ValidateUserEmail(string email) =>
		email == default
			? TypedResults.ValidationProblem(new Dictionary<string, string[]>
			{
				[nameof(CreateUserRequest.Email)] = ["An email is required."]
			})
			: null;

	private static UsersDto ToDto(Users item) =>
		new(item.UserId, item.Name, item.Email, item.Role);
}