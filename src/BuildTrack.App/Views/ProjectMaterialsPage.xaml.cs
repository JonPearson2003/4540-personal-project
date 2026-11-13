using BuildTrack.App.ViewModels;

namespace BuildTrack.App.Views;

public partial class ProjectMaterialsPage : ContentPage
{
	private readonly ProjectMaterialsViewModel viewModel;

	public ProjectMaterialsPage(ProjectMaterialsViewModel viewModel)
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
