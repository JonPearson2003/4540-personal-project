namespace BuildTrack.Contracts.Models;

public sealed record MaterialRequestDto(
	int ProjectId,
	int RequestedByUserId,
	int MaterialId,
	int QuantityRequested,
	DateTime RequestedAt
);
