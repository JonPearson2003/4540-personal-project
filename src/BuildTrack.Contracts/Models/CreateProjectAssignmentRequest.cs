using System.ComponentModel.DataAnnotations;

namespace BuildTrack.Contracts.Models;

/// <summary>Payload for assigning a user to a project.</summary>
public sealed record CreateProjectAssignmentRequest
{
	[Range(1, int.MaxValue)]
	public int ProjectId { get; init; }

	[Range(1, int.MaxValue)]
	public int UserId { get; init; }

	public DateTime AssignedAt { get; init; } = DateTime.UtcNow;
}
