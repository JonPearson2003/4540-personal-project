namespace BuildTrack.Api.Entities;

public sealed class MaterialUsage
{
	public int MaterialUsageId { get; set; }
	public int ProjectId { get; set; }
	public int UserId { get; set; }
	public int MaterialId { get; set; }
	public int QuantityUsed { get; set; }
	public DateTime UsedAt { get; set; }
}