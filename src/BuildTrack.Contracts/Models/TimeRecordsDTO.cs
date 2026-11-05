namespace BuildTrack.Contracts.Models;

public sealed record TimeRecordDto(
	int TimeRecordId,
	int UserId,
	DateTime StartTime,
	DateTime EndTime,
	TimeSpan Duration
);