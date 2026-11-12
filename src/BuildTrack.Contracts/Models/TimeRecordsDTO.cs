namespace BuildTrack.Contracts.Models;

public sealed record TimeRecordDto(
	int TimeRecordId,
	int UserId,
    int ProjectId,
	DateTime StartTime,
	DateTime EndTime,
	TimeSpan Duration
);