using BuildTrack.App.ViewModels;

namespace BuildTrack.App.Views;

public partial class ProjectStatusesPage : ContentPage
{
	private readonly ProjectStatusesViewModel viewModel;

	public ProjectStatusesPage(ProjectStatusesViewModel viewModel)
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
