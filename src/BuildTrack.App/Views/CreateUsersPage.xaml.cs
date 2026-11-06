using BuildTrack.App.ViewModels;

namespace BuildTrack.App.Views;

public partial class CreateUsersPage : ContentPage
{
	public CreateUsersPage(CreateUsersViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}