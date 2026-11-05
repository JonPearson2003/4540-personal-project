namespace BuildTrack.Api.Entities;

public sealed class Users
{
	public int UserId { get; set; }
	public required string Name { get; set; }
	public required string Email { get; set; }
	public required string Password { get; set; }
	public required string Role { get; set; }
}
