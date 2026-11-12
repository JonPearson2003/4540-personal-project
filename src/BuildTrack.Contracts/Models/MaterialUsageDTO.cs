namespace BuildTrack.Contracts.Models;

public sealed record MaterialUsageDto(
	int ProjectId,
	int UserId,
	int MaterialId,
	int QuantityUsed,
	DateTime AssignedAt
);
