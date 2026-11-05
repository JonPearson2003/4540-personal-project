namespace BuildTrack.Api.Entities;

public sealed class Projects
{
	public int ProjectId { get; set; }
	public required string Name { get; set; }
	public required string Description { get; set; }
	public required string Location { get; set; }
	public required DateTime StartDate { get; set; }
	public required DateTime EndDate { get; set; }
}