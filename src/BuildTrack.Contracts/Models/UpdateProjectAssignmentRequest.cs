namespace BuildTrack.Contracts.Models;

/// <summary>Payload for updating when a project assignment was created.</summary>
public sealed record UpdateProjectAssignmentRequest
{
	public DateTime AssignedAt { get; init; }
}
