namespace BuildTrack.Contracts.Models;

public sealed record UserDto(
	int UserId,
	string Name,
	string Email,
    string Password,
    string Role
    );
