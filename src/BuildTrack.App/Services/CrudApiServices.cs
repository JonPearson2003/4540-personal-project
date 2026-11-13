using System.Net;
using System.Net.Http.Json;
using BuildTrack.Contracts.Models;

namespace BuildTrack.App.Services;

public interface ICrudApiService<TDto, TCreateRequest, TUpdateRequest>
{
	Task<IReadOnlyList<TDto>> GetAllAsync(CancellationToken cancellationToken = default);
	Task<TDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
	Task<TDto> CreateAsync(TCreateRequest request, CancellationToken cancellationToken = default);
	Task UpdateAsync(string id, TUpdateRequest request, CancellationToken cancellationToken = default);
	Task DeleteAsync(string id, CancellationToken cancellationToken = default);
}

public abstract class CrudApiService<TDto, TCreateRequest, TUpdateRequest>(
	HttpClient httpClient,
	string resource) : ICrudApiService<TDto, TCreateRequest, TUpdateRequest>
{
	private string ResourcePath => $"api/buildtrack/{resource}";

	public async Task<IReadOnlyList<TDto>> GetAllAsync(CancellationToken cancellationToken = default) =>
		await httpClient.GetFromJsonAsync<List<TDto>>(ResourcePath, cancellationToken) ?? [];

	public async Task<TDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
	{
		using var response = await httpClient.GetAsync($"{ResourcePath}/{id}", cancellationToken);
		if (response.StatusCode == HttpStatusCode.NotFound)
			return default;

		await EnsureSuccessAsync(response, cancellationToken);
		return await response.Content.ReadFromJsonAsync<TDto>(cancellationToken);
	}

	public async Task<TDto> CreateAsync(
		TCreateRequest request, CancellationToken cancellationToken = default)
	{
		using var response = await httpClient.PostAsJsonAsync(ResourcePath, request, cancellationToken);
		await EnsureSuccessAsync(response, cancellationToken);
		return await response.Content.ReadFromJsonAsync<TDto>(cancellationToken)
			?? throw new HttpRequestException("The API returned an empty create response.");
	}

	public async Task UpdateAsync(
		string id, TUpdateRequest request, CancellationToken cancellationToken = default)
	{
		using var response = await httpClient.PutAsJsonAsync(
			$"{ResourcePath}/{id}", request, cancellationToken);
		await EnsureSuccessAsync(response, cancellationToken);
	}

	public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
	{
		using var response = await httpClient.DeleteAsync($"{ResourcePath}/{id}", cancellationToken);
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

public sealed class ProjectsApiService(HttpClient client)
	: CrudApiService<ProjectDto, CreateProjectRequest, UpdateProjectRequest>(client, "projects");

public sealed class TimeRecordsApiService(HttpClient client)
	: CrudApiService<TimeRecordDto, CreateTimeRecordRequest, UpdateTimeRecordRequest>(client, "timerecords");

public sealed class ProjectMaterialsApiService(HttpClient client)
	: CrudApiService<ProjectMaterialDto, CreateProjectMaterialRequest, UpdateProjectMaterialRequest>(client, "projectmaterials");

public sealed class ProjectStatusesApiService(HttpClient client)
	: CrudApiService<ProjectStatusDto, CreateProjectStatusRequest, UpdateProjectStatusRequest>(client, "projectstatuses");

public sealed class ProjectAssignmentsApiService(HttpClient client)
	: CrudApiService<ProjectAssignmentDto, CreateProjectAssignmentRequest, UpdateProjectAssignmentRequest>(client, "projectassignments");

public sealed class MaterialRequestsApiService(HttpClient client)
	: CrudApiService<MaterialRequestDto, CreateMaterialRequestRequest, UpdateMaterialRequestRequest>(client, "materialrequests");

public sealed class MaterialUsagesApiService(HttpClient client)
	: CrudApiService<MaterialUsageDto, CreateMaterialUsageRequest, UpdateMaterialUsageRequest>(client, "materialusages");

public sealed class ReceiptInvoicesApiService(HttpClient client)
	: CrudApiService<ReceiptInvoiceDto, CreateReceiptInvoiceRequest, UpdateReceiptInvoiceRequest>(client, "receiptinvoices");
