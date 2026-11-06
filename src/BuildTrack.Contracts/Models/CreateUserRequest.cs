using System.ComponentModel.DataAnnotations;

namespace BuildTrack.Contracts.Models;

public sealed class CreateUserRequest
{
	[Required]
	[StringLength(100, MinimumLength = 2)]
	public string Name { get; init; } = string.Empty;

	[Required]
	[EmailAddress]
	public string Email { get; init; } = string.Empty;

	[Required]
	[StringLength(100, MinimumLength = 6)]
	public string Password { get; init; } = string.Empty;

	[Required]
	[RegularExpression("^(worker|manager)$", ErrorMessage = "Role must be worker or manager.")]
	public string Role { get; init; } = string.Empty;
}