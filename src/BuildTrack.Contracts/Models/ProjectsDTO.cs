namespace BuildTrack.Contracts.Models;

public sealed record ProjectDto(
	int ProjectId,
	string Name,
    string Description,
    string Location,
    DateTime StartDate,
    DateTime EndDate
);