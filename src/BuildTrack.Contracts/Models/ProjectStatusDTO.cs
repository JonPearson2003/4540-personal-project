namespace BuildTrack.Contracts.Models;

public sealed record ProjectStatusDto(
    int ProjectStatusId,
	int ProjectId,
	byte[] ProgressPhoto,
    string status,
    DateTime date
);