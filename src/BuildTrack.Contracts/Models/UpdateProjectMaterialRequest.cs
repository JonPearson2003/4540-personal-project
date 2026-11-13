using System.ComponentModel.DataAnnotations;

namespace BuildTrack.Contracts.Models;

/// <summary>Payload for updating a material assigned to a project.</summary>
public sealed record UpdateProjectMaterialRequest
{
	[Range(1, int.MaxValue)]
	public int ProjectId { get; init; }

	[Required]
	public string Name { get; init; } = string.Empty;

	[Required]
	public string Description { get; init; } = string.Empty;

	[Range(typeof(decimal), "0", "79228162514264337593543950335")]
	public decimal Price { get; init; }

	[Range(0, int.MaxValue)]
	public int Quantity { get; init; }
}
