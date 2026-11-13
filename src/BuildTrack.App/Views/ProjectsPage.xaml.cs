using BuildTrack.App.ViewModels;

namespace BuildTrack.App.Views;

public partial class ProjectsPage : ContentPage
{
	private readonly ProjectsViewModel viewModel;

	public ProjectsPage(ProjectsViewModel viewModel)
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
