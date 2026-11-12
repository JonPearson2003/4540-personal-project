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
        
        group.MapGet("/projects", GetAllProjectsAsync);
        group.MapGet("/projects/{id:int}", GetByProjectIdAsync).WithName(nameof(GetByProjectIdAsync));
        group.MapPost("/projects", CreateProjectAsync);
        group.MapPut("/projects/{id:int}", UpdateProjectAsync);
        group.MapDelete("/projects/{id:int}", DeleteProjectAsync);
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

		var result = ToUserDto(entity);
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

	private static UsersDto ToUserDto(Users item) =>
		new(item.UserId, item.Name, item.Email, item.Role);

    	private static async Task<Ok<List<ProjectDto>>> GetAllProjectsAsync(
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var projects = await database.Projects.AsNoTracking()
			.OrderBy(project => project.Name)
			.Select(project => new ProjectDto(
				project.ProjectId, project.Name, project.Description, project.Location, project.StartDate, project.EndDate))
			.ToListAsync(cancellationToken);
		return TypedResults.Ok(projects);
	}

    	private static async Task<Results<Ok<ProjectDto>, NotFound>> GetByProjectIdAsync(
		int id,
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var item = await database.Projects.AsNoTracking()
			.Where(item => item.ProjectId == id)
			.Select(item => new ProjectDto(
				item.ProjectId, item.Name, item.Description, item.Location, item.StartDate, item.EndDate))
			.SingleOrDefaultAsync(cancellationToken);
		return item is null ? TypedResults.NotFound() : TypedResults.Ok(item);
	}

    	private static async Task<Results<Created<ProjectDto>, ValidationProblem>> CreateProjectAsync(
		CreateProjectRequest request,
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{

		var entity = new Projects
		{
			Name = request.Name.Trim(),
			Description = request.Description.Trim(),
			Location = request.Location.Trim(),
			StartDate = request.StartDate,
			EndDate = request.EndDate
		};
		database.Projects.Add(entity);
		await database.SaveChangesAsync(cancellationToken);

		var result = ToProjectDto(entity);
		return TypedResults.Created($"/api/projects/{entity.ProjectId}", result);
	}

    	private static async Task<Results<NoContent, NotFound, ValidationProblem>> UpdateProjectAsync(
		int id,
		UpdateProjectRequest request,
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var validationProblem = ValidateProjectName(request.Name);
		if (validationProblem is not null)
			return validationProblem;

		var entity = await database.Projects.FindAsync([id], cancellationToken);
		if (entity is null)
			return TypedResults.NotFound();

		entity.Name = request.Name.Trim();
		entity.Description = request.Description.Trim();
		entity.Location = request.Location.Trim();
		entity.StartDate = request.StartDate;
		entity.EndDate = request.EndDate;
		await database.SaveChangesAsync(cancellationToken);
		return TypedResults.NoContent();
	}

    	private static async Task<Results<NoContent, NotFound>> DeleteProjectAsync(
		int id,
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var entity = await database.Projects.FindAsync([id], cancellationToken);
		if (entity is null)
			return TypedResults.NotFound();

		database.Projects.Remove(entity);
		await database.SaveChangesAsync(cancellationToken);
		return TypedResults.NoContent();
	}

    	private static ProjectDto ToProjectDto(Projects item) =>
		new(item.ProjectId, item.Name, item.Description, item.Location, item.StartDate, item.EndDate);

    	private static ValidationProblem? ValidateProjectName(string name) =>
		name.Length < 2 || name.Length > 100
			? TypedResults.ValidationProblem(new Dictionary<string, string[]>
				{
					{ nameof(CreateProjectRequest.Name), new[] { "Name must be between 2 and 100 characters." } }
				})
			: null;


        

}