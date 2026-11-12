namespace BuildTrack.Contracts.Models;

public sealed record MaterialUsageDto(
	int MaterialUsageId,
	int ProjectId,
	int UserId,
	int MaterialId,
	int QuantityUsed,
	DateTime UsedAt
);
