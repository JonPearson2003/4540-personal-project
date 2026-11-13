using System.ComponentModel.DataAnnotations;

namespace BuildTrack.Contracts.Models;

/// <summary>Payload for updating a material request and its review details.</summary>
public sealed record UpdateMaterialRequestRequest
{
	[Range(1, int.MaxValue)]
	public int ProjectId { get; init; }

	[Range(1, int.MaxValue)]
	public int RequestedByUserId { get; init; }

	[Range(1, int.MaxValue)]
	public int MaterialId { get; init; }

	[Range(1, int.MaxValue)]
	public int QuantityRequested { get; init; }

	[Required]
	[StringLength(40, MinimumLength = 1)]
	public string Unit { get; init; } = string.Empty;

	[Required]
	[StringLength(20, MinimumLength = 1)]
	public string Priority { get; init; } = "Normal";

	[Required]
	[StringLength(30, MinimumLength = 1)]
	public string Status { get; init; } = "Pending";

	public string? Notes { get; init; }

	public DateTime RequestedAt { get; init; }

	public int? ReviewedByUserId { get; init; }

	public DateTime? ReviewedAt { get; init; }
}
