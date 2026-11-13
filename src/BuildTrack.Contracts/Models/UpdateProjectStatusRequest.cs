using System.ComponentModel.DataAnnotations;

namespace BuildTrack.Contracts.Models;

/// <summary>Payload for updating a project status entry.</summary>
public sealed record UpdateProjectStatusRequest
{
	[Range(1, int.MaxValue)]
	public int ProjectId { get; init; }

	public byte[] ProgressPhoto { get; init; } = [];

	[Required]
	[StringLength(100, MinimumLength = 1)]
	public string Status { get; init; } = string.Empty;

	public DateTime Date { get; init; }
}
