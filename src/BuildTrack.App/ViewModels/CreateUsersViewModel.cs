using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BuildTrack.App.Services;
using BuildTrack.Contracts.Models;

namespace BuildTrack.App.ViewModels;

public partial class CreateUsersViewModel(UsersApiService api) : ObservableObject
{
	[ObservableProperty] private string name = string.Empty;
	[ObservableProperty] private string email = string.Empty;
	[ObservableProperty] private string password = string.Empty;
	[ObservableProperty] private string role = string.Empty;
    [ObservableProperty] private bool isBusy;
	[ObservableProperty] private string errorMessage = string.Empty;

	public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);
	partial void OnErrorMessageChanged(string value) => OnPropertyChanged(nameof(HasError));

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
			await api.CreateAsync(new UpsertUsersRequest
			{
				Name = Name.Trim(),
				Email = Email.Trim(),
				Password = Password.Trim(),
				Role = Role.Trim()
			});
			await Shell.Current.GoToAsync("..");
		}
		catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException)
		{
			ErrorMessage = "The user could not be saved. Check the API and tunnel.";
		}
		finally
		{
			IsBusy = false;
		}
	}

	[RelayCommand]
	private static Task CancelAsync() => Shell.Current.GoToAsync("..");
}