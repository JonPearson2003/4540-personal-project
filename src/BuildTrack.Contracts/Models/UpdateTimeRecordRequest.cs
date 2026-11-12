using System.ComponentModel.DataAnnotations;

namespace BuildTrack.Contracts.Models;

public sealed class UpdateTimeRecordRequest
{

	[Required]
	public int ProjectId { get; init; }

	[Required]
	public DateTime StartTime { get; init; }

	[Required]
	public DateTime EndTime { get; init; }

	[Required]
	public TimeSpan Duration { get; init; }
}