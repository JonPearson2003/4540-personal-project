using System.ComponentModel.DataAnnotations;

namespace BuildTrack.Contracts.Models;
public sealed class UpdateProjectRequest
{
	[Required]
	[StringLength(100, MinimumLength = 2)]
	public string Name { get; init; } = string.Empty;

	[Required]
	[StringLength(250, MinimumLength = 10)]
	public string Description { get; init; } = string.Empty;

	[Required]
	[StringLength(100, MinimumLength = 6)]
	public string Location { get; init; } = string.Empty;

	[Required]
	public DateTime StartDate { get; init; }

	[Required]
	public DateTime EndDate { get; init; }
}
