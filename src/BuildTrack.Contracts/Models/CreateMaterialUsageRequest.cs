using System.ComponentModel.DataAnnotations;

namespace BuildTrack.Contracts.Models;

/// <summary>Payload for recording material used on a project.</summary>
public sealed record CreateMaterialUsageRequest
{
	[Range(1, int.MaxValue)]
	public int ProjectId { get; init; }

	[Range(1, int.MaxValue)]
	public int UserId { get; init; }

	[Range(1, int.MaxValue)]
	public int MaterialId { get; init; }

	[Range(1, int.MaxValue)]
	public int QuantityUsed { get; init; }

	public DateTime UsedAt { get; init; } = DateTime.UtcNow;
}
