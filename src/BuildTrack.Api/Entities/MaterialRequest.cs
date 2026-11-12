namespace BuildTrack.Api.Entities;

public sealed class MaterialRequest
{
	public int ProjectId { get; set; }
	public int RequestedByUserId { get; set; }
    public int MaterialId { get; set; }
	public DateTime RequestedAt { get; set; }
}