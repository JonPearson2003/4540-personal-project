using BuildTrack.App.ViewModels;

namespace BuildTrack.App.Views;

public partial class UsersPage : ContentPage
{
	private readonly UsersViewModel viewModel;

	public UsersPage(UsersViewModel viewModel)
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