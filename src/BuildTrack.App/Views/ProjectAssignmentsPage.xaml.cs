using BuildTrack.App.ViewModels;

namespace BuildTrack.App.Views;

public partial class ProjectAssignmentsPage : ContentPage
{
	private readonly ProjectAssignmentsViewModel viewModel;

	public ProjectAssignmentsPage(ProjectAssignmentsViewModel viewModel)
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
