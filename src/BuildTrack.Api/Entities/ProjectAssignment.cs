namespace BuildTrack.Api.Entities;

public sealed class ProjectAssignment
{
	public int ProjectId { get; set; }
	public int UserId { get; set; }
	public DateTime AssignedAt { get; set; }
}