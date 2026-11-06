using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BuildTrack.App.Services;
using BuildTrack.Contracts.Models;

namespace BuildTrack.App.ViewModels;

public partial class EditUsersViewModel(UsersApiService api)
	: ObservableObject, IQueryAttributable
{
	[ObservableProperty] private int userId;
	[ObservableProperty] private string name = string.Empty;
	[ObservableProperty] private string email = string.Empty;
	[ObservableProperty] private string role = string.Empty;
	[ObservableProperty] private bool isBusy;
	[ObservableProperty] private string errorMessage = string.Empty;

	public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);
	partial void OnErrorMessageChanged(string value) => OnPropertyChanged(nameof(HasError));

	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		if (query.TryGetValue("id", out var value)
			&& int.TryParse(value?.ToString(), out var id))
			UserId = id;
	}

	public async Task LoadAsync()
	{
		if (UserId <= 0 || IsBusy)
			return;

		try
		{
			IsBusy = true;
			ErrorMessage = string.Empty;
			var item = await api.GetByIdAsync(UserId);
			if (item is null)
			{
				ErrorMessage = "This user no longer exists.";
				return;
			}

			Name = item.Name;
			Email = item.Email;
			Role = item.Role;
		}
		catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException)
		{
			ErrorMessage = "The user could not be retrieved.";
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

		if (string.IsNullOrWhiteSpace(Name) || Name.Trim().Length is < 2 or > 100)
		{
			ErrorMessage = "Enter a name between 2 and 100 characters.";
			return;
		}

		try
		{
			IsBusy = true;
			ErrorMessage = string.Empty;
			await api.UpdateAsync(UserId, new UpsertUsersRequest
			{
				Name = Name.Trim(),
				Email = Email.Trim(),
				Role = Role.Trim()
			});
			await Shell.Current.GoToAsync("..");
		}
		catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException)
		{
			ErrorMessage = "The user could not be updated.";
		}
		finally
		{
			IsBusy = false;
		}
	}

	[RelayCommand]
	private static Task CancelAsync() => Shell.Current.GoToAsync("..");
}