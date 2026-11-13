using System.ComponentModel.DataAnnotations;

namespace BuildTrack.Contracts.Models;

/// <summary>Payload for updating a material usage record.</summary>
public sealed record UpdateMaterialUsageRequest
{
	[Range(1, int.MaxValue)]
	public int ProjectId { get; init; }

	[Range(1, int.MaxValue)]
	public int UserId { get; init; }

	[Range(1, int.MaxValue)]
	public int MaterialId { get; init; }

	[Range(1, int.MaxValue)]
	public int QuantityUsed { get; init; }

	public DateTime UsedAt { get; init; }
}
