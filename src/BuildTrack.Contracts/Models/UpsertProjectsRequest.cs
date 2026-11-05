using System.ComponentModel.DataAnnotations;

namespace BuildTrack.Contracts.Models;

public sealed class UpsertProjectsRequest
{
	[Required]
	[StringLength(100, MinimumLength = 2)]
	public string Name { get; init; } = string.Empty;

    [Required]
    [StringLength(200, MinimumLength = 2)]
    public string Location { get; init; } = string.Empty;

    [Required]
    public DateTime StartDate { get; init; }

    [Required]
    public DateTime EndDate { get; init; }
}