using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BuildTrack.App.Services;

namespace BuildTrack.App.ViewModels;

public abstract partial class CrudListViewModel<TDto, TCreateRequest, TUpdateRequest>(
	ICrudApiService<TDto, TCreateRequest, TUpdateRequest> api,
	string formPage,
	Func<TDto, string> getKey) : ObservableObject
{
	public ObservableCollection<TDto> Items { get; } = [];

	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private string errorMessage = string.Empty;

	public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

	partial void OnErrorMessageChanged(string value) => OnPropertyChanged(nameof(HasError));

	[RelayCommand]
	public async Task LoadAsync()
	{
		if (IsBusy)
			return;

		try
		{
			IsBusy = true;
			ErrorMessage = string.Empty;
			var items = await api.GetAllAsync();
			Items.Clear();
			foreach (var item in items)
				Items.Add(item);
		}
		catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException)
		{
			ErrorMessage = "Cannot reach the API. Check the API, tunnel, and app URL.";
		}
		finally
		{
			IsBusy = false;
		}
	}

	[RelayCommand]
	private Task AddAsync() => Shell.Current.GoToAsync(formPage);

	[RelayCommand]
	private Task EditAsync(TDto? item) =>
		item is null
			? Task.CompletedTask
			: Shell.Current.GoToAsync($"{formPage}?id={Uri.EscapeDataString(getKey(item))}");

	[RelayCommand]
	private async Task DeleteAsync(TDto? item)
	{
		if (item is null || IsBusy)
			return;

		try
		{
			IsBusy = true;
			ErrorMessage = string.Empty;
			await api.DeleteAsync(getKey(item));
			Items.Remove(item);
		}
		catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException)
		{
			ErrorMessage = "The item could not be deleted. It may still be referenced by other records.";
		}
		finally
		{
			IsBusy = false;
		}
	}
}

public abstract partial class CrudFormViewModel<TDto, TCreateRequest, TUpdateRequest>(
	ICrudApiService<TDto, TCreateRequest, TUpdateRequest> api) : ObservableObject, IQueryAttributable
{
	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(IsEditing))]
	private string? recordKey;

	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private string errorMessage = string.Empty;

	private bool isLoaded;

	public bool IsEditing => !string.IsNullOrWhiteSpace(RecordKey);
	public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

	partial void OnErrorMessageChanged(string value) => OnPropertyChanged(nameof(HasError));

	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		if (query.TryGetValue("id", out var value))
			RecordKey = value?.ToString();
		isLoaded = false;
	}

	public async Task LoadAsync()
	{
		if (isLoaded || !IsEditing || IsBusy)
			return;

		try
		{
			IsBusy = true;
			ErrorMessage = string.Empty;
			var item = await api.GetByIdAsync(RecordKey!);
			if (item is null)
			{
				ErrorMessage = "This item no longer exists.";
				return;
			}

			Apply(item);
			isLoaded = true;
		}
		catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException)
		{
			ErrorMessage = "The item could not be retrieved.";
		}
		finally
		{
			IsBusy = false;
		}
	}

	[RelayCommand]
	private async Task SaveAsync()
	{
		if (IsBusy)
			return;

		if (!Validate())
			return;

		try
		{
			IsBusy = true;
			ErrorMessage = string.Empty;
			if (IsEditing)
				await api.UpdateAsync(RecordKey!, CreateUpdateRequest());
			else
				await api.CreateAsync(CreateCreateRequest());
			await Shell.Current.GoToAsync("..");
		}
		catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException)
		{
			ErrorMessage = IsEditing
				? "The item could not be updated. Check the entered values and try again."
				: "The item could not be saved. Check the entered values and try again.";
		}
		finally
		{
			IsBusy = false;
		}
	}

	[RelayCommand]
	private static Task CancelAsync() => Shell.Current.GoToAsync("..");

	protected abstract void Apply(TDto item);
	protected abstract TCreateRequest CreateCreateRequest();
	protected abstract TUpdateRequest CreateUpdateRequest();
	protected virtual bool Validate() => true;
}
