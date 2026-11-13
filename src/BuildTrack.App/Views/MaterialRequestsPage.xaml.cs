using BuildTrack.App.ViewModels;

namespace BuildTrack.App.Views;

public partial class MaterialRequestsPage : ContentPage
{
	private readonly MaterialRequestsViewModel viewModel;

	public MaterialRequestsPage(MaterialRequestsViewModel viewModel)
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
