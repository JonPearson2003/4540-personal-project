namespace BuildTrack.Api.Entities;

public sealed class ProjectMaterials
{
	public int ProjectMaterialId { get; set; }
	public int ProjectId { get; set; }
	public required string Name { get; set; }
	public required string Description { get; set; }
	public required decimal Price { get; set; }
	public required int Quantity { get; set; }
}

