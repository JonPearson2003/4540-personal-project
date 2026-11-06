using BuildTrack.App.ViewModels;

namespace BuildTrack.App.Views;

public partial class EditUsersPage : ContentPage
{
	private readonly EditUsersViewModel viewModel;

	public EditUsersPage(EditUsersViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		this.viewModel = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await viewModel.LoadAsync();
	}
}