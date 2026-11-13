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
		var group = app.MapGroup("/api/buildtrack").WithTags("BuildTrack");
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

		group.MapGet("/timerecords", GetAllTimeRecordsAsync);
		group.MapGet("/timerecords/{id:int}", GetByTimeRecordIdAsync).WithName(nameof(GetByTimeRecordIdAsync));
		group.MapPost("/timerecords", CreateTimeRecordAsync);
		group.MapPut("/timerecords/{id:int}", UpdateTimeRecordAsync);
		group.MapDelete("/timerecords/{id:int}", DeleteTimeRecordAsync);

		group.MapGet("/projectmaterials", GetAllProjectMaterialsAsync);
		group.MapGet("/projectmaterials/{id:int}", GetByProjectMaterialIdAsync);
		group.MapPost("/projectmaterials", CreateProjectMaterialAsync);
		group.MapPut("/projectmaterials/{id:int}", UpdateProjectMaterialAsync);
		group.MapDelete("/projectmaterials/{id:int}", DeleteProjectMaterialAsync);

		group.MapGet("/projectstatuses", GetAllProjectStatusesAsync);
		group.MapGet("/projectstatuses/{id:int}", GetByProjectStatusIdAsync);
		group.MapPost("/projectstatuses", CreateProjectStatusAsync);
		group.MapPut("/projectstatuses/{id:int}", UpdateProjectStatusAsync);
		group.MapDelete("/projectstatuses/{id:int}", DeleteProjectStatusAsync);

		group.MapGet("/projectassignments", GetAllProjectAssignmentsAsync);
		group.MapGet("/projectassignments/{projectId:int}/{userId:int}", GetProjectAssignmentAsync);
		group.MapPost("/projectassignments", CreateProjectAssignmentAsync);
		group.MapPut("/projectassignments/{projectId:int}/{userId:int}", UpdateProjectAssignmentAsync);
		group.MapDelete("/projectassignments/{projectId:int}/{userId:int}", DeleteProjectAssignmentAsync);

		group.MapGet("/materialrequests", GetAllMaterialRequestsAsync);
		group.MapGet("/materialrequests/{id:int}", GetByMaterialRequestIdAsync);
		group.MapPost("/materialrequests", CreateMaterialRequestAsync);
		group.MapPut("/materialrequests/{id:int}", UpdateMaterialRequestAsync);
		group.MapDelete("/materialrequests/{id:int}", DeleteMaterialRequestAsync);

		group.MapGet("/materialusages", GetAllMaterialUsagesAsync);
		group.MapGet("/materialusages/{id:int}", GetByMaterialUsageIdAsync);
		group.MapPost("/materialusages", CreateMaterialUsageAsync);
		group.MapPut("/materialusages/{id:int}", UpdateMaterialUsageAsync);
		group.MapDelete("/materialusages/{id:int}", DeleteMaterialUsageAsync);

		group.MapGet("/receiptinvoices", GetAllReceiptInvoicesAsync);
		group.MapGet("/receiptinvoices/{id:int}", GetByReceiptInvoiceIdAsync);
		group.MapPost("/receiptinvoices", CreateReceiptInvoiceAsync);
		group.MapPut("/receiptinvoices/{id:int}", UpdateReceiptInvoiceAsync);
		group.MapDelete("/receiptinvoices/{id:int}", DeleteReceiptInvoiceAsync);
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
		return TypedResults.Created($"/api/buildtrack/users/{entity.UserId}", result);
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

	private static async Task<Results<NoContent, NotFound, Conflict>> DeleteUserAsync(
		int id,
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var entity = await database.Users.FindAsync([id], cancellationToken);
		if (entity is null)
			return TypedResults.NotFound();

		var isReferenced =
			await database.TimeRecords.AnyAsync(record => record.UserId == id, cancellationToken)
			|| await database.ProjectAssignments.AnyAsync(assignment => assignment.UserId == id, cancellationToken)
			|| await database.MaterialRequests.AnyAsync(request =>
				request.RequestedByUserId == id || request.ReviewedByUserId == id, cancellationToken)
			|| await database.MaterialUsages.AnyAsync(usage => usage.UserId == id, cancellationToken)
			|| await database.ReceiptInvoices.AnyAsync(receipt => receipt.UserId == id, cancellationToken);
		if (isReferenced)
			return TypedResults.Conflict();

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
		return TypedResults.Created($"/api/buildtrack/projects/{entity.ProjectId}", result);
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

	private static async Task<Results<NoContent, NotFound, Conflict>> DeleteProjectAsync(
		int id,
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var entity = await database.Projects.FindAsync([id], cancellationToken);
		if (entity is null)
			return TypedResults.NotFound();

		var isReferenced =
			await database.ProjectStatuses.AnyAsync(status => status.ProjectId == id, cancellationToken)
			|| await database.TimeRecords.AnyAsync(record => record.ProjectId == id, cancellationToken)
			|| await database.ProjectMaterials.AnyAsync(material => material.ProjectId == id, cancellationToken)
			|| await database.ProjectAssignments.AnyAsync(assignment => assignment.ProjectId == id, cancellationToken)
			|| await database.MaterialRequests.AnyAsync(request => request.ProjectId == id, cancellationToken)
			|| await database.MaterialUsages.AnyAsync(usage => usage.ProjectId == id, cancellationToken)
			|| await database.ReceiptInvoices.AnyAsync(receipt => receipt.ProjectId == id, cancellationToken);
		if (isReferenced)
			return TypedResults.Conflict();

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


        private static async Task<Ok<List<TimeRecordDto>>> GetAllTimeRecordsAsync(
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var timeRecords = await database.TimeRecords.AsNoTracking()
			.OrderBy(project => project.TimeRecordId)
			.Select(project => new TimeRecordDto(
				project.TimeRecordId, project.UserId,project.ProjectId, project.StartTime, project.EndTime, project.Duration))
			.ToListAsync(cancellationToken);
		return TypedResults.Ok(timeRecords);
	}

        private static async Task<Results<Ok<TimeRecordDto>, NotFound>> GetByTimeRecordIdAsync(
		int id,
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var timeRecords = await database.TimeRecords.AsNoTracking()
			.Where(item => item.TimeRecordId == id)
			.Select(item => new TimeRecordDto(
				item.TimeRecordId, item.UserId, item.ProjectId, item.StartTime, item.EndTime, item.Duration))
			.SingleOrDefaultAsync(cancellationToken);
		return timeRecords is null ? TypedResults.NotFound() : TypedResults.Ok(timeRecords);
	}

	private static async Task<Results<Created<TimeRecordDto>, NotFound, ValidationProblem>> CreateTimeRecordAsync(
		CreateTimeRecordRequest request,
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var validationProblem = ValidateTimeRecord(request.StartTime, request.EndTime);
		if (validationProblem is not null)
			return validationProblem;

		var userExists = await database.Users.AnyAsync(user => user.UserId == request.UserId, cancellationToken);
		if (!userExists)
			return TypedResults.NotFound();
		if (request.ProjectId is int projectId
			&& !await database.Projects.AnyAsync(project => project.ProjectId == projectId, cancellationToken))
			return TypedResults.NotFound();

		var entity = new TimeRecords
		{
			UserId = request.UserId,
			ProjectId = request.ProjectId,
			StartTime = request.StartTime,
			EndTime = request.EndTime,
			Duration = request.EndTime is DateTime completedAt
				? completedAt - request.StartTime
				: null
		};
		database.TimeRecords.Add(entity);
		await database.SaveChangesAsync(cancellationToken);

		var result = ToTimeRecordDto(entity);
		return TypedResults.Created($"/api/buildtrack/timerecords/{entity.TimeRecordId}", result);
	}

    	private static async Task<Results<NoContent, NotFound, ValidationProblem>> UpdateTimeRecordAsync(
		int id,
		UpdateTimeRecordRequest request,
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var validationProblem = ValidateTimeRecord(request.StartTime, request.EndTime);
		if (validationProblem is not null)
			return validationProblem;

		var entity = await database.TimeRecords.FindAsync([id], cancellationToken);
		if (entity is null)
			return TypedResults.NotFound();

		if (request.ProjectId is int projectId
			&& !await database.Projects.AnyAsync(project => project.ProjectId == projectId, cancellationToken))
			return TypedResults.NotFound();

		entity.ProjectId = request.ProjectId;
		entity.StartTime = request.StartTime;
		entity.EndTime = request.EndTime;
		entity.Duration = request.EndTime is DateTime completedAt
			? completedAt - request.StartTime
			: null;
		await database.SaveChangesAsync(cancellationToken);
		return TypedResults.NoContent();
	}

        private static async Task<Results<NoContent, NotFound>> DeleteTimeRecordAsync(
		int id,
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var entity = await database.TimeRecords.FindAsync([id], cancellationToken);
		if (entity is null)
			return TypedResults.NotFound();

		database.TimeRecords.Remove(entity);
		await database.SaveChangesAsync(cancellationToken);
		return TypedResults.NoContent();
	}


    	private static TimeRecordDto ToTimeRecordDto(TimeRecords item) =>
		new(item.TimeRecordId, item.UserId, item.ProjectId, item.StartTime, item.EndTime, item.Duration);

	private static ValidationProblem? ValidateTimeRecord(
		DateTime startTime,
		DateTime? endTime)
	{
		var errors = new Dictionary<string, string[]>();
		if (endTime is DateTime completedAt && completedAt <= startTime)
			errors[nameof(CreateTimeRecordRequest.EndTime)] = ["EndTime must be later than StartTime."];
		return errors.Count == 0 ? null : TypedResults.ValidationProblem(errors);
	}

        private static async Task<Ok<List<ProjectMaterialDto>>> GetAllProjectMaterialsAsync(
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var materials = await database.ProjectMaterials.AsNoTracking()
			.OrderBy(project => project.ProjectMaterialId)
			.Select(project => new ProjectMaterialDto(
				project.ProjectMaterialId, project.ProjectId, project.Name, project.Description, project.Price, project.Quantity))
			.ToListAsync(cancellationToken);
		return TypedResults.Ok(materials);
	}

        private static async Task<Results<Ok<ProjectMaterialDto>, NotFound>> GetByProjectMaterialIdAsync(
		int id,
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var materials = await database.ProjectMaterials.AsNoTracking()
			.Where(item => item.ProjectMaterialId == id)
			.Select(item => new ProjectMaterialDto(
				item.ProjectMaterialId, item.ProjectId, item.Name, item.Description, item.Price, item.Quantity))
			.SingleOrDefaultAsync(cancellationToken);
		return materials is null ? TypedResults.NotFound() : TypedResults.Ok(materials);
	}

	private static async Task<Results<Created<ProjectMaterialDto>, NotFound, ValidationProblem>> CreateProjectMaterialAsync(
		CreateProjectMaterialRequest request,
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var validationProblem = ValidateProjectMaterial(request.Name, request.Description, request.Price, request.Quantity);
		if (validationProblem is not null)
			return validationProblem;
		if (!await database.Projects.AnyAsync(project => project.ProjectId == request.ProjectId, cancellationToken))
			return TypedResults.NotFound();
		var entity = new ProjectMaterials
		{
			ProjectId = request.ProjectId,
			Name = request.Name.Trim(),
			Description = request.Description.Trim(),
			Price = request.Price,
			Quantity = request.Quantity
		};
		database.ProjectMaterials.Add(entity);
		await database.SaveChangesAsync(cancellationToken);

		var result = ToProjectMaterialDto(entity);
		return TypedResults.Created($"/api/buildtrack/projectmaterials/{entity.ProjectMaterialId}", result);
	}

	private static async Task<Results<NoContent, NotFound, Conflict, ValidationProblem>> UpdateProjectMaterialAsync(
		int id,
		UpdateProjectMaterialRequest request,
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var validationProblem = ValidateProjectMaterial(request.Name, request.Description, request.Price, request.Quantity);
		if (validationProblem is not null)
			return validationProblem;

		var entity = await database.ProjectMaterials.FindAsync([id], cancellationToken);
		if (entity is null)
			return TypedResults.NotFound();

		if (!await database.Projects.AnyAsync(project => project.ProjectId == request.ProjectId, cancellationToken))
			return TypedResults.NotFound();
		if (request.ProjectId != entity.ProjectId
			&& (await database.MaterialRequests.AnyAsync(
					materialRequest => materialRequest.MaterialId == id, cancellationToken)
				|| await database.MaterialUsages.AnyAsync(
					materialUsage => materialUsage.MaterialId == id, cancellationToken)))
			return TypedResults.Conflict();

		entity.ProjectId = request.ProjectId;
		entity.Name = request.Name.Trim();
		entity.Description = request.Description.Trim();
		entity.Price = request.Price;
		entity.Quantity = request.Quantity;
		await database.SaveChangesAsync(cancellationToken);
		return TypedResults.NoContent();
	}

	private static async Task<Results<NoContent, NotFound, Conflict>> DeleteProjectMaterialAsync(
		int id,
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var entity = await database.ProjectMaterials.FindAsync([id], cancellationToken);
		if (entity is null)
			return TypedResults.NotFound();

		var isReferenced =
			await database.MaterialRequests.AnyAsync(request => request.MaterialId == id, cancellationToken)
			|| await database.MaterialUsages.AnyAsync(usage => usage.MaterialId == id, cancellationToken);
		if (isReferenced)
			return TypedResults.Conflict();

		database.ProjectMaterials.Remove(entity);
		await database.SaveChangesAsync(cancellationToken);
		return TypedResults.NoContent();
	}


	private static ProjectMaterialDto ToProjectMaterialDto(ProjectMaterials item) =>
		new(item.ProjectMaterialId, item.ProjectId, item.Name, item.Description, item.Price, item.Quantity);

	private static ValidationProblem? ValidateProjectMaterial(string name, string description, decimal price, int quantity)
	{
		var errors = new Dictionary<string, string[]>();
		if (string.IsNullOrWhiteSpace(name))
			errors[nameof(CreateProjectMaterialRequest.Name)] = ["A material name is required."];
		if (string.IsNullOrWhiteSpace(description))
			errors[nameof(CreateProjectMaterialRequest.Description)] = ["A material description is required."];
		if (price < 0)
			errors[nameof(CreateProjectMaterialRequest.Price)] = ["Price cannot be negative."];
		if (quantity < 0)
			errors[nameof(CreateProjectMaterialRequest.Quantity)] = ["Quantity cannot be negative."];
		return errors.Count == 0 ? null : TypedResults.ValidationProblem(errors);
	}

	private static async Task<Ok<List<ProjectStatusDto>>> GetAllProjectStatusesAsync(
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var items = await database.ProjectStatuses.AsNoTracking()
			.OrderBy(item => item.Date)
			.Select(item => new ProjectStatusDto(
				item.ProjectStatusId, item.ProjectId, item.ProgressPhoto, item.Status, item.Date))
			.ToListAsync(cancellationToken);
		return TypedResults.Ok(items);
	}

	private static async Task<Results<Ok<ProjectStatusDto>, NotFound>> GetByProjectStatusIdAsync(
		int id,
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var item = await database.ProjectStatuses.AsNoTracking()
			.Where(item => item.ProjectStatusId == id)
			.Select(item => new ProjectStatusDto(
				item.ProjectStatusId, item.ProjectId, item.ProgressPhoto, item.Status, item.Date))
			.SingleOrDefaultAsync(cancellationToken);
		return item is null ? TypedResults.NotFound() : TypedResults.Ok(item);
	}

	private static async Task<Results<Created<ProjectStatusDto>, NotFound>> CreateProjectStatusAsync(
		CreateProjectStatusRequest request,
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		if (!await database.Projects.AnyAsync(project => project.ProjectId == request.ProjectId, cancellationToken))
			return TypedResults.NotFound();

		var entity = new ProjectStatus
		{
			ProjectId = request.ProjectId,
			ProgressPhoto = request.ProgressPhoto,
			Status = request.Status.Trim(),
			Date = request.Date
		};
		database.ProjectStatuses.Add(entity);
		await database.SaveChangesAsync(cancellationToken);
		var result = ToProjectStatusDto(entity);
		return TypedResults.Created($"/api/buildtrack/projectstatuses/{entity.ProjectStatusId}", result);
	}

	private static async Task<Results<NoContent, NotFound>> UpdateProjectStatusAsync(
		int id,
		UpdateProjectStatusRequest request,
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var entity = await database.ProjectStatuses.FindAsync([id], cancellationToken);
		if (entity is null)
			return TypedResults.NotFound();
		if (!await database.Projects.AnyAsync(project => project.ProjectId == request.ProjectId, cancellationToken))
			return TypedResults.NotFound();

		entity.ProjectId = request.ProjectId;
		entity.ProgressPhoto = request.ProgressPhoto;
		entity.Status = request.Status.Trim();
		entity.Date = request.Date;
		await database.SaveChangesAsync(cancellationToken);
		return TypedResults.NoContent();
	}

	private static async Task<Results<NoContent, NotFound>> DeleteProjectStatusAsync(
		int id,
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var entity = await database.ProjectStatuses.FindAsync([id], cancellationToken);
		if (entity is null)
			return TypedResults.NotFound();

		database.ProjectStatuses.Remove(entity);
		await database.SaveChangesAsync(cancellationToken);
		return TypedResults.NoContent();
	}

	private static ProjectStatusDto ToProjectStatusDto(ProjectStatus item) =>
		new(item.ProjectStatusId, item.ProjectId, item.ProgressPhoto, item.Status, item.Date);

	private static async Task<Ok<List<ProjectAssignmentDto>>> GetAllProjectAssignmentsAsync(
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var items = await database.ProjectAssignments.AsNoTracking()
			.OrderBy(item => item.ProjectId)
			.ThenBy(item => item.UserId)
			.Select(item => new ProjectAssignmentDto(item.ProjectId, item.UserId, item.AssignedAt))
			.ToListAsync(cancellationToken);
		return TypedResults.Ok(items);
	}

	private static async Task<Results<Ok<ProjectAssignmentDto>, NotFound>> GetProjectAssignmentAsync(
		int projectId,
		int userId,
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var item = await database.ProjectAssignments.AsNoTracking()
			.Where(item => item.ProjectId == projectId && item.UserId == userId)
			.Select(item => new ProjectAssignmentDto(item.ProjectId, item.UserId, item.AssignedAt))
			.SingleOrDefaultAsync(cancellationToken);
		return item is null ? TypedResults.NotFound() : TypedResults.Ok(item);
	}

	private static async Task<Results<Created<ProjectAssignmentDto>, NotFound, Conflict>> CreateProjectAssignmentAsync(
		CreateProjectAssignmentRequest request,
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var projectExists = await database.Projects.AnyAsync(
			project => project.ProjectId == request.ProjectId, cancellationToken);
		var userExists = await database.Users.AnyAsync(
			user => user.UserId == request.UserId, cancellationToken);
		if (!projectExists || !userExists)
			return TypedResults.NotFound();

		if (await database.ProjectAssignments.AnyAsync(
			item => item.ProjectId == request.ProjectId && item.UserId == request.UserId, cancellationToken))
			return TypedResults.Conflict();

		var entity = new ProjectAssignment
		{
			ProjectId = request.ProjectId,
			UserId = request.UserId,
			AssignedAt = request.AssignedAt
		};
		database.ProjectAssignments.Add(entity);
		await database.SaveChangesAsync(cancellationToken);
		var result = new ProjectAssignmentDto(entity.ProjectId, entity.UserId, entity.AssignedAt);
		return TypedResults.Created(
			$"/api/buildtrack/projectassignments/{entity.ProjectId}/{entity.UserId}", result);
	}

	private static async Task<Results<NoContent, NotFound>> UpdateProjectAssignmentAsync(
		int projectId,
		int userId,
		UpdateProjectAssignmentRequest request,
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var entity = await database.ProjectAssignments.FindAsync([projectId, userId], cancellationToken);
		if (entity is null)
			return TypedResults.NotFound();

		entity.AssignedAt = request.AssignedAt;
		await database.SaveChangesAsync(cancellationToken);
		return TypedResults.NoContent();
	}

	private static async Task<Results<NoContent, NotFound>> DeleteProjectAssignmentAsync(
		int projectId,
		int userId,
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var entity = await database.ProjectAssignments.FindAsync([projectId, userId], cancellationToken);
		if (entity is null)
			return TypedResults.NotFound();

		database.ProjectAssignments.Remove(entity);
		await database.SaveChangesAsync(cancellationToken);
		return TypedResults.NoContent();
	}

	private static async Task<Ok<List<MaterialRequestDto>>> GetAllMaterialRequestsAsync(
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var items = await database.MaterialRequests.AsNoTracking()
			.OrderByDescending(item => item.RequestedAt)
			.Select(item => new MaterialRequestDto(
				item.MaterialRequestId, item.ProjectId, item.RequestedByUserId, item.MaterialId,
				item.QuantityRequested, item.Unit, item.Priority, item.Status, item.Notes,
				item.RequestedAt, item.ReviewedByUserId, item.ReviewedAt))
			.ToListAsync(cancellationToken);
		return TypedResults.Ok(items);
	}

	private static async Task<Results<Ok<MaterialRequestDto>, NotFound>> GetByMaterialRequestIdAsync(
		int id,
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var item = await database.MaterialRequests.AsNoTracking()
			.Where(item => item.MaterialRequestId == id)
			.Select(item => new MaterialRequestDto(
				item.MaterialRequestId, item.ProjectId, item.RequestedByUserId, item.MaterialId,
				item.QuantityRequested, item.Unit, item.Priority, item.Status, item.Notes,
				item.RequestedAt, item.ReviewedByUserId, item.ReviewedAt))
			.SingleOrDefaultAsync(cancellationToken);
		return item is null ? TypedResults.NotFound() : TypedResults.Ok(item);
	}

	private static async Task<Results<Created<MaterialRequestDto>, NotFound>> CreateMaterialRequestAsync(
		CreateMaterialRequestRequest request,
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		if (!await database.Projects.AnyAsync(project => project.ProjectId == request.ProjectId, cancellationToken)
			|| !await database.Users.AnyAsync(user => user.UserId == request.RequestedByUserId, cancellationToken)
			|| !await database.ProjectMaterials.AnyAsync(material =>
				material.ProjectId == request.ProjectId && material.ProjectMaterialId == request.MaterialId,
				cancellationToken)
			|| (request.ReviewedByUserId is int reviewedByUserId
				&& !await database.Users.AnyAsync(user => user.UserId == reviewedByUserId, cancellationToken)))
			return TypedResults.NotFound();

		var entity = new MaterialRequest
		{
			ProjectId = request.ProjectId,
			RequestedByUserId = request.RequestedByUserId,
			MaterialId = request.MaterialId,
			QuantityRequested = request.QuantityRequested,
			Unit = request.Unit.Trim(),
			Priority = request.Priority.Trim(),
			Status = request.Status.Trim(),
			Notes = request.Notes?.Trim(),
			RequestedAt = request.RequestedAt,
			ReviewedByUserId = request.ReviewedByUserId,
			ReviewedAt = request.ReviewedAt
		};
		database.MaterialRequests.Add(entity);
		await database.SaveChangesAsync(cancellationToken);
		var result = ToMaterialRequestDto(entity);
		return TypedResults.Created($"/api/buildtrack/materialrequests/{entity.MaterialRequestId}", result);
	}

	private static async Task<Results<NoContent, NotFound>> UpdateMaterialRequestAsync(
		int id,
		UpdateMaterialRequestRequest request,
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var entity = await database.MaterialRequests.FindAsync([id], cancellationToken);
		if (entity is null)
			return TypedResults.NotFound();
		if (!await database.Projects.AnyAsync(project => project.ProjectId == request.ProjectId, cancellationToken)
			|| !await database.Users.AnyAsync(user => user.UserId == request.RequestedByUserId, cancellationToken)
			|| !await database.ProjectMaterials.AnyAsync(material =>
				material.ProjectId == request.ProjectId && material.ProjectMaterialId == request.MaterialId,
				cancellationToken)
			|| (request.ReviewedByUserId is int reviewedByUserId
				&& !await database.Users.AnyAsync(user => user.UserId == reviewedByUserId, cancellationToken)))
			return TypedResults.NotFound();

		entity.ProjectId = request.ProjectId;
		entity.RequestedByUserId = request.RequestedByUserId;
		entity.MaterialId = request.MaterialId;
		entity.QuantityRequested = request.QuantityRequested;
		entity.Unit = request.Unit.Trim();
		entity.Priority = request.Priority.Trim();
		entity.Status = request.Status.Trim();
		entity.Notes = request.Notes?.Trim();
		entity.RequestedAt = request.RequestedAt;
		entity.ReviewedByUserId = request.ReviewedByUserId;
		entity.ReviewedAt = request.ReviewedAt;
		await database.SaveChangesAsync(cancellationToken);
		return TypedResults.NoContent();
	}

	private static async Task<Results<NoContent, NotFound>> DeleteMaterialRequestAsync(
		int id,
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var entity = await database.MaterialRequests.FindAsync([id], cancellationToken);
		if (entity is null)
			return TypedResults.NotFound();

		database.MaterialRequests.Remove(entity);
		await database.SaveChangesAsync(cancellationToken);
		return TypedResults.NoContent();
	}

	private static MaterialRequestDto ToMaterialRequestDto(MaterialRequest item) =>
		new(item.MaterialRequestId, item.ProjectId, item.RequestedByUserId, item.MaterialId,
			item.QuantityRequested, item.Unit, item.Priority, item.Status, item.Notes,
			item.RequestedAt, item.ReviewedByUserId, item.ReviewedAt);

	private static async Task<Ok<List<MaterialUsageDto>>> GetAllMaterialUsagesAsync(
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var items = await database.MaterialUsages.AsNoTracking()
			.OrderByDescending(item => item.UsedAt)
			.Select(item => new MaterialUsageDto(
				item.MaterialUsageId, item.ProjectId, item.UserId, item.MaterialId, item.QuantityUsed, item.UsedAt))
			.ToListAsync(cancellationToken);
		return TypedResults.Ok(items);
	}

	private static async Task<Results<Ok<MaterialUsageDto>, NotFound>> GetByMaterialUsageIdAsync(
		int id,
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var item = await database.MaterialUsages.AsNoTracking()
			.Where(item => item.MaterialUsageId == id)
			.Select(item => new MaterialUsageDto(
				item.MaterialUsageId, item.ProjectId, item.UserId, item.MaterialId, item.QuantityUsed, item.UsedAt))
			.SingleOrDefaultAsync(cancellationToken);
		return item is null ? TypedResults.NotFound() : TypedResults.Ok(item);
	}

	private static async Task<Results<Created<MaterialUsageDto>, NotFound>> CreateMaterialUsageAsync(
		CreateMaterialUsageRequest request,
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		if (!await database.Projects.AnyAsync(project => project.ProjectId == request.ProjectId, cancellationToken)
			|| !await database.Users.AnyAsync(user => user.UserId == request.UserId, cancellationToken)
			|| !await database.ProjectMaterials.AnyAsync(material =>
				material.ProjectId == request.ProjectId && material.ProjectMaterialId == request.MaterialId,
				cancellationToken))
			return TypedResults.NotFound();

		var entity = new MaterialUsage
		{
			ProjectId = request.ProjectId,
			UserId = request.UserId,
			MaterialId = request.MaterialId,
			QuantityUsed = request.QuantityUsed,
			UsedAt = request.UsedAt
		};
		database.MaterialUsages.Add(entity);
		await database.SaveChangesAsync(cancellationToken);
		var result = ToMaterialUsageDto(entity);
		return TypedResults.Created($"/api/buildtrack/materialusages/{entity.MaterialUsageId}", result);
	}

	private static async Task<Results<NoContent, NotFound>> UpdateMaterialUsageAsync(
		int id,
		UpdateMaterialUsageRequest request,
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var entity = await database.MaterialUsages.FindAsync([id], cancellationToken);
		if (entity is null)
			return TypedResults.NotFound();
		if (!await database.Projects.AnyAsync(project => project.ProjectId == request.ProjectId, cancellationToken)
			|| !await database.Users.AnyAsync(user => user.UserId == request.UserId, cancellationToken)
			|| !await database.ProjectMaterials.AnyAsync(material =>
				material.ProjectId == request.ProjectId && material.ProjectMaterialId == request.MaterialId,
				cancellationToken))
			return TypedResults.NotFound();

		entity.ProjectId = request.ProjectId;
		entity.UserId = request.UserId;
		entity.MaterialId = request.MaterialId;
		entity.QuantityUsed = request.QuantityUsed;
		entity.UsedAt = request.UsedAt;
		await database.SaveChangesAsync(cancellationToken);
		return TypedResults.NoContent();
	}

	private static async Task<Results<NoContent, NotFound>> DeleteMaterialUsageAsync(
		int id,
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var entity = await database.MaterialUsages.FindAsync([id], cancellationToken);
		if (entity is null)
			return TypedResults.NotFound();

		database.MaterialUsages.Remove(entity);
		await database.SaveChangesAsync(cancellationToken);
		return TypedResults.NoContent();
	}

	private static MaterialUsageDto ToMaterialUsageDto(MaterialUsage item) =>
		new(item.MaterialUsageId, item.ProjectId, item.UserId, item.MaterialId, item.QuantityUsed, item.UsedAt);

	private static async Task<Ok<List<ReceiptInvoiceDto>>> GetAllReceiptInvoicesAsync(
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var items = await database.ReceiptInvoices.AsNoTracking()
			.OrderByDescending(item => item.PurchaseDate)
			.Select(item => new ReceiptInvoiceDto(
				item.ReceiptInvoiceId, item.ProjectId, item.UserId, item.Vendor, item.Amount,
				item.PurchaseDate, item.InvoiceImage))
			.ToListAsync(cancellationToken);
		return TypedResults.Ok(items);
	}

	private static async Task<Results<Ok<ReceiptInvoiceDto>, NotFound>> GetByReceiptInvoiceIdAsync(
		int id,
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var item = await database.ReceiptInvoices.AsNoTracking()
			.Where(item => item.ReceiptInvoiceId == id)
			.Select(item => new ReceiptInvoiceDto(
				item.ReceiptInvoiceId, item.ProjectId, item.UserId, item.Vendor, item.Amount,
				item.PurchaseDate, item.InvoiceImage))
			.SingleOrDefaultAsync(cancellationToken);
		return item is null ? TypedResults.NotFound() : TypedResults.Ok(item);
	}

	private static async Task<Results<Created<ReceiptInvoiceDto>, NotFound>> CreateReceiptInvoiceAsync(
		CreateReceiptInvoiceRequest request,
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		if (!await database.Projects.AnyAsync(project => project.ProjectId == request.ProjectId, cancellationToken)
			|| !await database.Users.AnyAsync(user => user.UserId == request.UserId, cancellationToken))
			return TypedResults.NotFound();

		var entity = new ReceiptInvoice
		{
			ProjectId = request.ProjectId,
			UserId = request.UserId,
			Vendor = request.Vendor.Trim(),
			Amount = request.Amount,
			PurchaseDate = request.PurchaseDate,
			InvoiceImage = request.InvoiceImage
		};
		database.ReceiptInvoices.Add(entity);
		await database.SaveChangesAsync(cancellationToken);
		var result = ToReceiptInvoiceDto(entity);
		return TypedResults.Created($"/api/buildtrack/receiptinvoices/{entity.ReceiptInvoiceId}", result);
	}

	private static async Task<Results<NoContent, NotFound>> UpdateReceiptInvoiceAsync(
		int id,
		UpdateReceiptInvoiceRequest request,
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var entity = await database.ReceiptInvoices.FindAsync([id], cancellationToken);
		if (entity is null)
			return TypedResults.NotFound();
		if (!await database.Projects.AnyAsync(project => project.ProjectId == request.ProjectId, cancellationToken)
			|| !await database.Users.AnyAsync(user => user.UserId == request.UserId, cancellationToken))
			return TypedResults.NotFound();

		entity.ProjectId = request.ProjectId;
		entity.UserId = request.UserId;
		entity.Vendor = request.Vendor.Trim();
		entity.Amount = request.Amount;
		entity.PurchaseDate = request.PurchaseDate;
		entity.InvoiceImage = request.InvoiceImage;
		await database.SaveChangesAsync(cancellationToken);
		return TypedResults.NoContent();
	}

	private static async Task<Results<NoContent, NotFound>> DeleteReceiptInvoiceAsync(
		int id,
		BuildTrackDbContext database,
		CancellationToken cancellationToken)
	{
		var entity = await database.ReceiptInvoices.FindAsync([id], cancellationToken);
		if (entity is null)
			return TypedResults.NotFound();

		database.ReceiptInvoices.Remove(entity);
		await database.SaveChangesAsync(cancellationToken);
		return TypedResults.NoContent();
	}

	private static ReceiptInvoiceDto ToReceiptInvoiceDto(ReceiptInvoice item) =>
		new(item.ReceiptInvoiceId, item.ProjectId, item.UserId, item.Vendor, item.Amount,
			item.PurchaseDate, item.InvoiceImage);

}