using BuildTrack.Api.Entities;
using Microsoft.AspNetCore.Identity;

namespace BuildTrack.Api.Security;

internal static class UserPasswordHashing
{
	private const string HashPrefix = "identity-v1:";

	public static string Hash(
		Users user,
		string password,
		IPasswordHasher<Users> passwordHasher) =>
		$"{HashPrefix}{passwordHasher.HashPassword(user, password)}";

	public static bool IsHashed(string storedPassword) =>
		storedPassword.StartsWith(HashPrefix, StringComparison.Ordinal);
}
