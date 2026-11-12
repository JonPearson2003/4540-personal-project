namespace BuildTrack.Contracts.Models;

public sealed record ProjectAssignmentDto(
	int ProjectId,
	int UserId,
	DateTime AssignedAt
);
