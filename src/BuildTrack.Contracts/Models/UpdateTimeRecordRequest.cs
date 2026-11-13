using System.ComponentModel.DataAnnotations;

namespace BuildTrack.Contracts.Models;

public sealed class UpdateTimeRecordRequest
{
	public int? ProjectId { get; init; }

	[Required]
	public DateTime StartTime { get; init; }

	public DateTime? EndTime { get; init; }

	public TimeSpan? Duration { get; init; }
}