using BuildTrack.App.ViewModels;

namespace BuildTrack.App.Views;

public partial class MaterialUsagesPage : ContentPage
{
	private readonly MaterialUsagesViewModel viewModel;

	public MaterialUsagesPage(MaterialUsagesViewModel viewModel)
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
