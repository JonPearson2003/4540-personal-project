namespace BuildTrack.Api.Entities;

public sealed class MaterialUsage
{
	public int ProjectId { get; set; }
	public int UserId { get; set; }
    public int MaterialId { get; set; }
    public int QuantityUsed { get; set; }
	public DateTime AssignedAt { get; set; }
}