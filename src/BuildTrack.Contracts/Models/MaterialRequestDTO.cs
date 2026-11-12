namespace BuildTrack.Contracts.Models;

public sealed record MaterialRequestDto(
	int MaterialRequestId,
	int ProjectId,
	int RequestedByUserId,
	int MaterialId,
	int QuantityRequested,
	string Unit,
	string Priority,
	string Status,
	string? Notes,
	DateTime RequestedAt,
	int? ReviewedByUserId,
	DateTime? ReviewedAt
);
