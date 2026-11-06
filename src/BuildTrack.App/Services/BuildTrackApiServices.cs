using System.Net;
using System.Net.Http.Json;
using BuildTrack.Contracts.Models;

namespace BuildTrack.App.Services;

public sealed class UsersApiService(HttpClient httpClient)
{
	public async Task<IReadOnlyList<UsersDto>> GetAllAsync(
		CancellationToken cancellationToken = default) =>
		await httpClient.GetFromJsonAsync<List<UsersDto>>(
			"api/buildtrack/users", cancellationToken) ?? [];

	public async Task<UsersDto?> GetByIdAsync(
		int id, CancellationToken cancellationToken = default)
	{
		using var response = await httpClient.GetAsync(
			$"api/buildtrack/users/{id}", cancellationToken);
		if (response.StatusCode == HttpStatusCode.NotFound)
			return null;

		await EnsureSuccessAsync(response, cancellationToken);
		return await response.Content.ReadFromJsonAsync<UsersDto>(cancellationToken);
	}

	public async Task<UsersDto> CreateAsync(
		UpsertUsersRequest request, CancellationToken cancellationToken = default)
	{
		using var response = await httpClient.PostAsJsonAsync(
			"api/buildtrack/users", request, cancellationToken);
		await EnsureSuccessAsync(response, cancellationToken);
		return await response.Content.ReadFromJsonAsync<UsersDto>(cancellationToken)
			?? throw new HttpRequestException("The API returned an empty create response.");
	}

	public async Task UpdateAsync(
		int id, UpsertUsersRequest request, CancellationToken cancellationToken = default)
	{
		using var response = await httpClient.PutAsJsonAsync(
			$"api/buildtrack/users/{id}", request, cancellationToken);
		await EnsureSuccessAsync(response, cancellationToken);
	}

	public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
	{
		using var response = await httpClient.DeleteAsync(
			$"api/buildtrack/users/{id}", cancellationToken);
		await EnsureSuccessAsync(response, cancellationToken);
	}

	private static async Task EnsureSuccessAsync(
		HttpResponseMessage response, CancellationToken cancellationToken)
	{
		if (response.IsSuccessStatusCode)
			return;

		var body = await response.Content.ReadAsStringAsync(cancellationToken);
		throw new HttpRequestException(
			$"API request failed with {(int)response.StatusCode} {response.ReasonPhrase}. {body}");
	}
}