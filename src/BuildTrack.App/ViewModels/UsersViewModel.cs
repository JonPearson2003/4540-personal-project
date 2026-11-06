using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BuildTrack.App.Services;
using BuildTrack.App.Views;
using BuildTrack.Contracts.Models;

namespace BuildTrack.App.ViewModels;

public partial class UsersViewModel(UsersApiService api) : ObservableObject
{
	public ObservableCollection<UsersDto> Users { get; } = [];

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
			Users.Clear();
			foreach (var item in items)
				Users.Add(item);
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
	private static Task AddAsync() => Shell.Current.GoToAsync(nameof(AddUsersPage));

	[RelayCommand]
	private static Task EditAsync(UsersDto? item) =>
		item is null
			? Task.CompletedTask
			: Shell.Current.GoToAsync($"{nameof(EditUsersPage)}?id={item.UserId}");

	[RelayCommand]
	private async Task DeleteAsync(UsersDto? item)
	{
		if (item is null || IsBusy)
			return;

		try
		{
			IsBusy = true;
			ErrorMessage = string.Empty;
			await api.DeleteAsync(item.UserId);
			Users.Remove(item);
		}
		catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException)
		{
			ErrorMessage = "The user could not be deleted. Refresh and try again.";
		}
		finally
		{
			IsBusy = false;
		}
	}
}