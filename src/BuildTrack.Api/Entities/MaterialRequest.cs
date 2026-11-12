namespace BuildTrack.Api.Entities;

public sealed class MaterialRequest
{
	public int MaterialRequestId { get; set; }
	public int ProjectId { get; set; }
	public int RequestedByUserId { get; set; }
	public int MaterialId { get; set; }
	public int QuantityRequested { get; set; }
	public required string Unit { get; set; }
	public string Priority { get; set; } = "Normal";
	public string Status { get; set; } = "Pending";
	public string? Notes { get; set; }
	public DateTime RequestedAt { get; set; }
	public int? ReviewedByUserId { get; set; }
	public DateTime? ReviewedAt { get; set; }
}