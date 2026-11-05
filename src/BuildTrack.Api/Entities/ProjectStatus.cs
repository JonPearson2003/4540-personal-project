namespace BuildTrack.Api.Entities;

public sealed class ProjectStatus
{
	public int ProjectStatusId { get; set; }
	public int ProjectId { get; set; }
	public byte[] ProgressPhoto { get; set; } = Array.Empty<byte>();
	public required string Status { get; set; }
	public required DateTime Date { get; set; }
}
