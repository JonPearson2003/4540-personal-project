namespace BuildTrack.Contracts.Models;

public sealed record UsersDto(
	int UserId,
	string Name,
	string Email,
    string Password,
    string Role
    );
